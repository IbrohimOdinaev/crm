
using crm.DataAccess.Entities;

namespace crm.DataAccess.IRepositories;

public interface IWalletRepository
{
    Task AddAsync(Wallet wallet, CancellationToken token = default);

    Task<Wallet?> GetByIdAsync(Guid walletId, CancellationToken token = default);

    Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken token = default);

    Task TopUpAsync(Wallet wallet, decimal amount, CancellationToken token = default);

    Task SubtractAsync(Wallet wallet, decimal amount, CancellationToken token = default);
    Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken token = default);
}