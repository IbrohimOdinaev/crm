using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;


namespace crm.BusinessLogic.Extensions;

public static class WalletExtension
{
    public static Wallet ToDbWallet(this WalletDto walletDto)
    => new()
    {
        Id = walletDto.Id,
        Balance = walletDto.Balance,
        UserId = walletDto.UserId
    };

    public static WalletDto ToWalletDto(this Wallet wallet)
    => new()
    {
        Id = wallet.Id,
        Balance = wallet.Balance,
        UserId = wallet.UserId
    };

}