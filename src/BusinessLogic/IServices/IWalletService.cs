using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;

namespace crm.BusinessLogic.IServices;

public interface IWalletService
{
    Task<Guid> CreateWalletAsync(WalletDto walletDto, CancellationToken token = default);
    Task<bool> AddMoneyAsync(Guid walletId, decimal amount, CancellationToken token = default);

    Task<bool> SubtractMoneyAsync(Guid walletId, decimal amount, CancellationToken token = default);

    Task<WalletDto?> GetByIdAsync(Guid walletId, CancellationToken token = default);

    Task<WalletDto?> GetByUserIdAsync(Guid userId, CancellationToken token = default);

    Task<IEnumerable<WalletDto>> GetAllAsync();
}