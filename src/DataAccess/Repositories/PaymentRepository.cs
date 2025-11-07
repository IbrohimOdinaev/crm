
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;

namespace crm.DataAccess.Repositories;

public class PaymentRepository : IPaymentRepository
{
    public Task<Payment?> GetByIdAsync(Guid paymentId, CancellationToken token)
        => Task.FromResult(DataStorage.Payments.FirstOrDefault(payment => payment.Id == paymentId));

    public Task AddAsync(Payment payment, CancellationToken token)
    {
        DataStorage.Payments.Add(payment);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Payment>> GetAllAsync(CancellationToken token)
        => Task.FromResult((IEnumerable<Payment>)DataStorage.Payments);

    public Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId, CancellationToken token)
        => Task.FromResult(DataStorage.Payments.Where(payment => payment.UserId == userId));
    
    public Task UpdateAsync(Payment payment, Payment newPayment, CancellationToken token)
    {
        payment.Amount = newPayment.Amount;
        payment.PaymentStatus = newPayment.PaymentStatus;

        return Task.CompletedTask;
    }
}