using crm.BusinessLogic.IServices;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Extensions;

namespace crm.BusinessLogic.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;

    public WalletService(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Guid> CreateWalletAsync(WalletDto walletDto, CancellationToken token)
    {
        Wallet wallet = walletDto.ToDbWallet();

        await _walletRepository.AddAsync(wallet);

        return walletDto.Id;
    }
    public async Task<bool> AddMoneyAsync(Guid walletId, decimal amount, CancellationToken token)
    {
        Wallet? wallet = await _walletRepository.GetByIdAsync(walletId, token);

        if (wallet == null) return false;

        await _walletRepository.TopUpAsync(wallet, amount, token);

        return true;
    }

    public async Task<bool> SubtractMoneyAsync(Guid walletId, decimal amount, CancellationToken token)
    {
        Wallet? wallet = await _walletRepository.GetByIdAsync(walletId, token);

        if (wallet == null || wallet.Balance < amount) return false;

        await _walletRepository.SubtractAsync(wallet, amount, token);

        return true;
    }

    public async Task<WalletDto?> GetByIdAsync(Guid walletId, CancellationToken token)
    {
        Wallet? wallet = await _walletRepository.GetByIdAsync(walletId);

        return wallet is null ? null : wallet.ToWalletDto();
    }

    public async Task<WalletDto?> GetByUserIdAsync(Guid userId, CancellationToken token)
    {
        Wallet? wallet = await _walletRepository.GetByUserIdAsync(userId);

        return wallet is null ? null : wallet.ToWalletDto();
    }

    public async Task<IEnumerable<WalletDto>> GetAllAsync()
        => (await _walletRepository.GetAllAsync()).Select(wallet => wallet.ToWalletDto());
    
}