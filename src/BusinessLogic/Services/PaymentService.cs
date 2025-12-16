using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Helpers.Extensions;
using crm.BusinessLogic.IServices;
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.Enums;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;

namespace crm.BusinessLogic.Services;

public class PaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletService _walletService;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IWalletService walletService)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _walletService = walletService;
    }
    public async Task<Guid> CreatePaymentAsync(PaymentDto paymentDto, CancellationToken token)
    {
        await _paymentRepository.AddAsync(paymentDto.ToDbPayment(), token);

        return paymentDto.Id;
    }

    public async Task<bool> ProcessPaymentAsync(Guid paymentId, CancellationToken token)
    {
        Payment payment = (await _paymentRepository.GetByIdAsync(paymentId))!;
        Order order = (await _orderRepository.GetByIdAsync(payment.OrderId))!;
        User user = (await _userRepository.GetByIdAsync(payment.UserId))!;

        if (await _walletService.SubtractMoneyAsync(user.Wallet!.Id, order.TotalAmount) == false)
        {
            payment.PaymentStatus = PaymentStatus.Failed;
            return false;
        }

        payment.Amount = order.TotalAmount;
        order.PaymentId = payment.Id;
        payment.PaymentStatus = PaymentStatus.Completed;
        order.PaymentStatus = PaymentStatus.Completed;
        order.OrderStatus = OrderStatus.Delivered;

        return true;
    }

    public async Task<bool> RefundPaymentAsync(Guid paymentId, CancellationToken token)
    {
        Payment? payment = await _paymentRepository.GetByIdAsync(paymentId);

        if (payment is null || payment.PaymentStatus != PaymentStatus.Completed) return false;

        Order order = (await _orderRepository.GetByIdAsync(payment.OrderId))!;

        await _walletService.AddMoneyAsync((await _userRepository.GetByIdAsync(payment.UserId))!.Wallet!.Id, order.TotalAmount, token);

        payment.PaymentStatus = PaymentStatus.Refunded;
        order.PaymentStatus = PaymentStatus.Refunded;
        order.OrderStatus = OrderStatus.Returned;

        return true;
    }

    public async Task<PaymentStatus?> GetPaymentStatusAsync(Guid paymentId, CancellationToken token)
    {
        Payment? payment = await _paymentRepository.GetByIdAsync(paymentId, token);

        return payment == null ? null : payment.PaymentStatus;
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid paymentId, CancellationToken token)
    {
        Payment? payment = await _paymentRepository.GetByIdAsync(paymentId);

        if (payment is null) return null;

        return payment.ToPaymentDto();
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken token)
        => (await _paymentRepository.GetAllAsync()).Select(payment => payment.ToPaymentDto());


    public async Task<IEnumerable<PaymentDto>> GetByUserIdAsync(Guid userId, CancellationToken token)
        => (await _paymentRepository.GetByUserIdAsync(userId)).Select(payment => payment.ToPaymentDto());

    public async Task UpdateAsync(PaymentDto paymentDto, CancellationToken token)
    {
        Payment payment = (await _paymentRepository.GetByIdAsync(paymentDto.Id))!;

        await _paymentRepository.UpdateAsync(payment, paymentDto.ToDbPayment());
    }
}