using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Services;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Entities;
using Moq;
using crm.BusinessLogic.IServices;
using crm.DataAccess.Enums;
namespace test.BusinessLogicTests.ServicesTests;

public class PaymentServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMoq;
    private readonly Mock<IUserRepository> _userRepositoryMoq;
    private readonly Mock<IPaymentRepository> _paymentRepositoryMoq;
    private readonly PaymentService _paymentService;
    private readonly Mock<IWalletService> _walletServiceMoq;

    public PaymentServiceTests()
    {
        _orderRepositoryMoq = new();
        _userRepositoryMoq = new();
        _paymentRepositoryMoq = new();
        _walletServiceMoq = new();
        _paymentService = new(
            _paymentRepositoryMoq.Object,
            _orderRepositoryMoq.Object,
            _userRepositoryMoq.Object,
            _walletServiceMoq.Object);
    }

    [Fact]
    public async Task CreatePaymentAsync_AddNewPaymentToDb()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        PaymentDto paymentDto = new();
        _paymentRepositoryMoq.Setup(r => r.AddAsync(It.IsAny<Payment>(), token)).Returns(Task.CompletedTask);

        // Act
        await _paymentService.CreatePaymentAsync(paymentDto, token);

        // Assert
        _paymentRepositoryMoq.Verify(r => r.AddAsync(It.IsAny<Payment>(), token), Times.Once);
    }


    [Fact]
    public async Task ProcessPaymentAsync_NotEnoughMoney_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Order order = new();
        User user = new() { Wallet = new() };
        Payment payment = new() { OrderId = order.Id, UserId = user.Id };
        _orderRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(order);
        _userRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(user);
        _paymentRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(payment);
        _walletServiceMoq.Setup(r => r.SubtractMoneyAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), token)).ReturnsAsync(false);

        // Act
        var result = await _paymentService.ProcessPaymentAsync(payment.Id, token);

        // Assert
        Assert.False(result);
        Assert.Equal(PaymentStatus.Failed, payment.PaymentStatus);
        _orderRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
        _userRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
        _walletServiceMoq.Verify(r => r.SubtractMoneyAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), token), Times.Once);
    }

    [Fact]
    public async Task ProcessPaymentAsync_CorrecDataAndEnoughMoney_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Order order = new();
        User user = new() { Wallet = new() };
        Payment payment = new() { OrderId = order.Id, UserId = user.Id };
        _orderRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(order);
        _userRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(user);
        _paymentRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(payment);
        _walletServiceMoq.Setup(r => r.SubtractMoneyAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), token)).ReturnsAsync(true);

        // Act
        var result = await _paymentService.ProcessPaymentAsync(payment.Id, token);

        // Assert
        Assert.True(result);
        Assert.Equal(PaymentStatus.Completed, payment.PaymentStatus);
        Assert.Equal(PaymentStatus.Completed, order.PaymentStatus);
        _orderRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
        _userRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
        _walletServiceMoq.Verify(r => r.SubtractMoneyAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), token));
    }

    [Fact]
    public async Task RefundPaymentasync_PaymentNotFound_ReturnsFasle()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        _paymentRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Payment?)null);

        // Act
        var result = await _paymentService.RefundPaymentAsync(Guid.NewGuid(), token);

        // Assert
        Assert.False(result);
        _paymentRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task RefundPyamentAsync_CorrectPayment_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        User user = new() { Wallet = new() { Balance = 300 } };
        Order order = new() { PaymentStatus = PaymentStatus.Completed, TotalAmount = 1000 };
        Payment payment = new() { UserId = user.Id, OrderId = order.Id, PaymentStatus = PaymentStatus.Completed };
        _paymentRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(payment);
        _walletServiceMoq.Setup(r => r.AddMoneyAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), token)).ReturnsAsync(true);
        _userRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(user);
        _orderRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(order);

        // Act
        var result = await _paymentService.RefundPaymentAsync(payment.Id, token);

        // Assert
        Assert.True(result);
        _paymentRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
        _walletServiceMoq.Verify(r => r.AddMoneyAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), token), Times.Once);
        _userRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
        _orderRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task GetPaymentStatusAsync_PyamentNotFound_ReturnsNull()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Payment payment = new();
        _paymentRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Payment?)null);

        // Act
        var result = await _paymentService.GetPaymentStatusAsync(payment.Id, token);

        // Assert
        Assert.Null(result);
        _paymentRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

    [Fact]
    public async Task GetPaymentStatusAsync_CorrecPaymentId_ReturnsNull()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Payment payment = new() { PaymentStatus = PaymentStatus.Pending };
        _paymentRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(payment);

        // Act
        var result = await _paymentService.GetPaymentStatusAsync(payment.Id, token);

        // Assert
        Assert.Equal(payment.PaymentStatus, result);
        _paymentRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

    
}