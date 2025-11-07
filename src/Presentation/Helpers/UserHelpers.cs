using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Extensions;
using crm.BusinessLogic.IServices;
using crm.BusinessLogic.Services;
using crm.DataAccess.Enums;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;
using static crm.Presentation.Helpers.BaseHelpers;
namespace crm.Presentation.Helpers;

public static class UserHelpers
{
    public static async Task<UserDto> GetUserDtoAsync(UserRole userRole, IWalletService _walletService, CancellationToken token = default)
    {
        Guid Id = Guid.NewGuid();

        string name = (await GetStringAsync<string>("Name", token))!;

        string email = (await GetStringAsync<string>("Email", token))!;

        string passwordHash = (await GetStringAsync<string>("Password", token))!;

        return new()
        {
            Id = Id,
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            UserRole = userRole,
            CreatedAt = DateTime.Now,
            IsActive = true
        };
    }

    public static async Task<LoginDto> GetLoginDataAsync(CancellationToken token = default)
    {
        string name = (await GetStringAsync<string>("Name", token))!;

        string passwordHash = (await GetStringAsync<string>("Password", token))!;

        return new(name, passwordHash);
    }

    public static async Task<ChangePasswordDto> GetChangePasswordDtoAsync(CancellationToken token = default)
    {
        string name = (await GetStringAsync<string>("Name", token))!;

        string password = (await GetStringAsync<string>("Password", token))!;

        string newPassword = (await GetStringAsync<string>("New Password", token))!;

        return new(name, password, newPassword);
    }
}