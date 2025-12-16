namespace crm.BusinessLogic.IServices;

using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;
using crm.DataAccess.Enums;

public interface IUserService
{
    Task<Guid> RegisterAsync(UserDto userDto, CancellationToken token = default);

    Task<bool> LoginAsync(LoginDto loginDto, CancellationToken token = default);

    Task<bool> UpdateUserAsync(UserDto userDto, CancellationToken token = default);

    Task<bool> DeactivateUserAsync(Guid userId, CancellationToken token = default);

    Task<List<UserDto>> GetAllUsersAsync(CancellationToken token = default);

    Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken token = default);

    Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken token = default);

    Task<UserDto?> GetByNameAsync(string userName, CancellationToken token = default);

    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken token = default);

    Task UpdateAsync(UserDto userDto, CancellationToken token = default);

    Task<UserDto> UpdateDtoDataAsync(Guid UserId, CancellationToken token = default);
}