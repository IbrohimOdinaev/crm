using crm.BusinessLogic.Services;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using crm.BusinessLogic.Dtos;
using Moq;
using crm.BusinessLogic.Helpers.Extensions;
using crm.DataAccess.DataBase;
using crm.BusinessLogic.IServices;

namespace test.BusinessLogicTests.ServicesTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMoq;
    private readonly Mock<IWalletService> _walletServiceMoq;
    private readonly Mock<IWalletRepository> _walletRepositoryMoq;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMoq = new();
        _walletServiceMoq = new();
        _walletRepositoryMoq = new();
        _userService = new(_userRepositoryMoq.Object, _walletServiceMoq.Object, _walletRepositoryMoq.Object);
    }

    [Fact]
    public async Task RegisterAsync_AddsNewUserToDb()
    {
        //Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        UserDto userDto = new() { Id = Guid.NewGuid() };
        User user = await userDto.ToDbUser();
        _userRepositoryMoq.Setup(r => r.AddAsync(It.IsAny<User>(), token)).Returns(Task.CompletedTask);

        //Act
        await _userService.RegisterAsync(userDto, token);

        // Assert
        _userRepositoryMoq.Verify(r => r.AddAsync(It.IsAny<User>(), token), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        User user = new() { Name = "Siyovush", PasswordHash = "1111" , IsActive = true};
        LoginDto loginDto = new(user.Name, user.PasswordHash);
        _userRepositoryMoq.Setup(r => r.GetByNameAsync(user.Name, token)).Returns(Task.FromResult(user)!);

        // Act
        var result = await _userService.LoginAsync(loginDto, token);

        // Assert
        Assert.True(result);
        _userRepositoryMoq.Verify(r => r.GetByNameAsync(user.Name, token), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        User user = new() { Name = "Siyovush", PasswordHash = "1111" };
        LoginDto loginDto = new("Ibrohim", "1111");
        _userRepositoryMoq.Setup(r => r.GetByNameAsync(It.IsAny<string>(), token)).ReturnsAsync((User?)null);

        //Act
        var result = await _userService.LoginAsync(loginDto, token);

        //Assert
        Assert.False(result);
        _userRepositoryMoq.Verify(r => r.GetByNameAsync(It.IsAny<string>(), token), Times.Once);
    }

    [Fact]
    public async Task LoginASync_WrongPassword_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        User user = new() { Name = "Siyovush", PasswordHash = "1111" };
        LoginDto loginDto = new(user.Name, "2222");
        _userRepositoryMoq.Setup(r => r.GetByNameAsync(It.IsAny<string>(), token)).ReturnsAsync(user);

        // Act
        var result = await _userService.LoginAsync(loginDto, token);

        // Assert
        Assert.False(result);
        _userRepositoryMoq.Verify(r => r.GetByNameAsync(It.IsAny<string>(), token), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_UserNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        User user = new() { Id = Guid.NewGuid() };
        UserDto newUserDto = new() { Id = Guid.NewGuid() };
        _userRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.UpdateUserAsync(newUserDto, token);

        // Assert
        Assert.False(result);
        _userRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once());

    }

    [Fact]
    public async Task UpdateUserAsync_CorrectUserDto_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        User user = new() { Id = Guid.NewGuid(), Name = "Siyovush" };
        string newName = "Ibrohim";
        UserDto newUserDto = new() { Id = user.Id, Name = newName };
        _userRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(user);
        _userRepositoryMoq.Setup(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<User>(), token)).Returns(Task.CompletedTask);
        

        // Act
        var result = await _userService.UpdateUserAsync(newUserDto, token);

        // Assert
        Assert.True(result);
        _userRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
        _userRepositoryMoq.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<User>(), token), Times.Once);
    }

    [Fact]
    public async Task DeactivateUserAsync_UserNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        User user = new() { Id = userId };
        _userRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.DeactivateUserAsync(Guid.NewGuid(), token);

        // Assert
        Assert.False(result);
        _userRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task DeactivateUserAsync_CorrectUser_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        User user = new() { Id = userId };
        _userRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(user);

        // Act
        var result = await _userService.DeactivateUserAsync(user.Id, token);

        // Assert
        Assert.True(result);
        _userRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_UserNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        string userName = "Siyovush";
        string userPassword = "1111";
        User user = new() { Name = userName, PasswordHash = userPassword };
        ChangePasswordDto changePasswordDto = new("Ibrohim", "1111", "2222");
        _userRepositoryMoq.Setup(r => r.GetByNameAsync(It.IsAny<string>(), token)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.ChangePasswordAsync(changePasswordDto, token);

        // Assert
        Assert.False(result);
        _userRepositoryMoq.Verify(r => r.GetByNameAsync(It.IsAny<string>(), token), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_WrongPassword_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        string userName = "Siyovush";
        string userPassword = "1111";
        User user = new() { Name = userName, PasswordHash = userPassword };
        ChangePasswordDto changePasswordDto = new("Siyovush", "3333", "2222");
        _userRepositoryMoq.Setup(r => r.GetByNameAsync(It.IsAny<string>(), token)).ReturnsAsync(user);

        // Act
        var result = await _userService.ChangePasswordAsync(changePasswordDto, token);

        // Assert
        Assert.False(result);
        _userRepositoryMoq.Verify(r => r.GetByNameAsync(It.IsAny<string>(), token), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_CorrectPassword_ReturnsTrueAndChangesUserPassword()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        string userName = "Siyovush";
        string userPassword = "1111";
        User user = new() { Name = userName, PasswordHash = userPassword };
        ChangePasswordDto changePasswordDto = new(userName, userPassword, "1234");
        _userRepositoryMoq.Setup(r => r.GetByNameAsync(It.IsAny<string>(), token)).ReturnsAsync(user);

        // Act
        var result = await _userService.ChangePasswordAsync(changePasswordDto, token);

        // Assert
        Assert.True(result);
        Assert.Equal("1234", user.PasswordHash);
        _userRepositoryMoq.Verify(r => r.GetByNameAsync(It.IsAny<string>(), token), Times.Once);
    }
}