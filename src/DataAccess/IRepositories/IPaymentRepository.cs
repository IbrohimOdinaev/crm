
using crm.DataAccess.Entities;

namespace crm.DataAccess.IRepositories;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid paymentId, CancellationToken token = default);

    Task AddAsync(Payment payment, CancellationToken token = default);

    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken token = default);

    Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId, CancellationToken token = default);

    Task UpdateAsync(Payment payment, Payment newPayment, CancellationToken token = default);
}