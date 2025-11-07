namespace crm.BusinessLogic.Services;

using crm.BusinessLogic.IServices;
using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;
using crm.DataAccess.DataBase;
using crm.BusinessLogic.Helpers.Extensions;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Enums;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IWalletService _walletService;
    private readonly IWalletRepository _walletRepository;

    public UserService(IUserRepository userRepository, IWalletService walletService, IWalletRepository walletRepository)
    {
        _userRepository = userRepository;
        _walletService = walletService;
        _walletRepository = walletRepository;
    }

    public async Task<Guid> RegisterAsync(UserDto userDto, CancellationToken token)
    {
        WalletDto walletDto = new(Guid.NewGuid(), userDto.Id);

        Wallet wallet = (await _walletRepository.GetByIdAsync(await _walletService.CreateWalletAsync(walletDto)))!;

        User user = new()
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            CreatedAt = DateTime.Now,
            IsActive = true,
            PasswordHash = userDto.PasswordHash,
            UserRole = userDto.UserRole,
            Wallet = wallet
        };

        await _userRepository.AddAsync(user);

        return user.Id;
    }

    public async Task<bool> LoginAsync(LoginDto loginDto, CancellationToken token)
    {
        User? user = await _userRepository.GetByNameAsync(loginDto.Name, token);
        
        if (user is null || user.PasswordHash != loginDto.Password || user.IsActive == false) return false;

        return true;
    }

    public async Task<bool> UpdateUserAsync(UserDto userDto, CancellationToken token)
    {
        User? user = await _userRepository.GetByIdAsync(userDto.Id, token);

        if (user is null) return false;

        await _userRepository.UpdateAsync(user, await userDto.ToDbUser());

        return true;
    }
    public async Task<bool> DeactivateUserAsync(Guid userId, CancellationToken token)
    {
        User? user = await _userRepository.GetByIdAsync(userId, token);

        if (user is not null)
        {
            user.IsActive = false;

            return true;
        }

        return false;
    }

    public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken token) => (await _userRepository.GetAllAsync(token)).Select(user => user.ToUserDto()).ToList();

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken token)
    {
        User? user = await _userRepository.GetByNameAsync(changePasswordDto.Name, token);

        if (user is null || user.PasswordHash != changePasswordDto.PasswordHash) return false;

        user.PasswordHash = changePasswordDto.NewPassword;

        return true;
    }

    public async Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken token)
    {
        User? user = await _userRepository.GetByIdAsync(userId);

        return user is null ? null : user.ToUserDto();
    }

    public async Task<UserDto?> GetByNameAsync(string userName, CancellationToken token)
    {
        User? user = await _userRepository.GetByNameAsync(userName);

        return user is null ? null : user.ToUserDto();
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken token)
        => (await _userRepository.GetAllAsync()).Select(user => user.ToUserDto());

    public async Task UpdateAsync(UserDto userDto, CancellationToken token)
    {
        User user = (await _userRepository.GetByIdAsync(userDto.Id))!;

        await _userRepository.UpdateAsync(user, await userDto.ToDbUser());
    }

    public async Task<UserDto> UpdateDtoDataAsync(Guid userId, CancellationToken token)
        => (await _userRepository.GetByIdAsync(userId))!.ToUserDto();
}