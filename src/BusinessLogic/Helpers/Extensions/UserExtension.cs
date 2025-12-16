namespace crm.BusinessLogic.Helpers.Extensions;

using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Extensions;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;

public static class userExtension
{
    public static readonly IWalletRepository _walletRepository = new WalletRepository();
    public async static Task<User> ToDbUser(this UserDto userDto)
    => new()
    {
        Id = userDto.Id,

        Name = userDto.Name,

        Email = userDto.Email,

        PasswordHash = userDto.PasswordHash,

        UserRole = userDto.UserRole,

        CreatedAt = userDto.CreatedAt,

        LastLoginAt = userDto.LastLoginAt,

        IsActive = userDto.IsActive,

        Wallet = userDto.Wallet == null ? null : await _walletRepository.GetByIdAsync(userDto.Wallet.Id)
    };

    public static UserDto ToUserDto(this User dbUser)
    => new()
    {
        Id = dbUser.Id,

        Name = dbUser.Name,

        Email = dbUser.Email,

        PasswordHash = dbUser.PasswordHash,

        UserRole = dbUser.UserRole,

        CreatedAt = dbUser.CreatedAt,

        LastLoginAt = dbUser.LastLoginAt,

        IsActive = dbUser.IsActive,

        Wallet = dbUser.Wallet?.ToWalletDto()
    };
}