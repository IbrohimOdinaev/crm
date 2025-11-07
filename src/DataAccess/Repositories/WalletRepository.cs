
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;

namespace crm.DataAccess.Repositories;

public class WalletRepository : IWalletRepository
{
    public Task AddAsync(Wallet wallet, CancellationToken token)
    {
        DataStorage.Wallets.Add(wallet);

        return Task.CompletedTask;
    }
    public Task<Wallet?> GetByIdAsync(Guid walletId, CancellationToken token)
        => Task.FromResult(DataStorage.Wallets.FirstOrDefault(wallet => wallet.Id == walletId));

    public Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken token)
        => Task.FromResult(DataStorage.Wallets.FirstOrDefault(wallet => wallet.UserId == userId));

    public Task TopUpAsync(Wallet wallet, decimal amount, CancellationToken token)
    {
        wallet.Balance += amount;

        return Task.CompletedTask;
    }

    public Task SubtractAsync(Wallet wallet, decimal amount, CancellationToken token)
    {
        wallet.Balance -= amount;

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken token)
        => Task.FromResult((IEnumerable<Wallet>)DataStorage.Wallets);
}