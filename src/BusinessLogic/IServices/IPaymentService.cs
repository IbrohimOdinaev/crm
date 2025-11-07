namespace crm.BusinessLogic.IServices;

using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;
using crm.DataAccess.Enums;
public interface IPaymentService
{
    Task<Guid> CreatePaymentAsync(PaymentDto paymentDto, CancellationToken token = default);
    Task<bool> ProcessPaymentAsync( Guid paymentId, CancellationToken token = default);
    Task<bool> RefundPaymentAsync(Guid paymentId, CancellationToken token = default);
    Task<PaymentStatus?> GetPaymentStatusAsync(Guid orderId, CancellationToken token = default);
    Task<PaymentDto?> GetByIdAsync(Guid paymentId, CancellationToken token = default);
    Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken token = default);
    Task<IEnumerable<PaymentDto>> GetByUserIdAsync(Guid userId, CancellationToken token = default);
    Task UpdateAsync(PaymentDto paymentDto, CancellationToken token = default);
}