using crm.DataAccess.IRepositories;
using Moq;
using crm.BusinessLogic.Services;
using crm.DataAccess.Entities;
using crm.DataAccess.DataBase;
using crm.BusinessLogic.Dtos;
namespace test.BusinessLogicTests.ServicesTests;

public class WalletServiceTests
{
    private readonly Mock<IWalletRepository> _walletRepositoryMoq;
    private readonly WalletService _walletService;

    public WalletServiceTests()
    {
        _walletRepositoryMoq = new();
        _walletService = new(_walletRepositoryMoq.Object);
    }

    [Fact]
    public async Task CreateWalletAsync()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        Wallet wallet = new(userId);
        WalletDto walletDto = new() { UserId = userId };
        _walletRepositoryMoq.Setup(r => r.AddAsync(It.IsAny<Wallet>(), token)).Returns(Task.CompletedTask);

        // Act
        await _walletService.CreateWalletAsync(walletDto, token);

        // Assert
        _walletRepositoryMoq.Verify(r => r.AddAsync(It.IsAny<Wallet>(), token), Times.Once);
    }

    [Fact]
    public async Task AddMoneyAsync_WalletNotFound_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        int amount = 200;
        Wallet wallet = new(userId);
        _walletRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Wallet?)null);

        // Act
        var result = await _walletService.AddMoneyAsync(wallet.Id, amount, token);

        // Assert
        Assert.False(result);
        _walletRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task AddMoneyAsync_CorrectWalletId_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        Wallet wallet = new(userId) { Balance = 1000 };
        _walletRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(wallet);
        _walletRepositoryMoq.Setup(r => r.TopUpAsync(It.IsAny<Wallet>(), It.IsAny<decimal>(), token)).Returns(Task.CompletedTask);

        // Act
        var result = await _walletService.AddMoneyAsync(wallet.Id, 300, token);

        // Assert
        Assert.True(result);
        _walletRepositoryMoq.Verify(r => r.TopUpAsync(It.IsAny<Wallet>(), It.IsAny<decimal>(), token), Times.Once);
    }

    [Fact]
    public async Task SubtractMoneyAsync_WalletNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        Wallet wallet = new(userId) { Balance = 100 };
        _walletRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Wallet?)null);

        // Act
        var result = await _walletService.SubtractMoneyAsync(wallet.Id, 300, token);

        // Assert
        Assert.False(result);
        _walletRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

    [Fact]
    public async Task SubtractMoneyAsync_NotEnoughMoney_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        Wallet wallet = new(userId) { Balance = 100 };
        _walletRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(wallet);

        // Act
        var result = await _walletService.SubtractMoneyAsync(wallet.Id, 300, token);

        // Assert
        Assert.False(result);
        _walletRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

    [Fact]
    public async Task SubtractMoneyAsync_EnoughMoney_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid userId = Guid.NewGuid();
        Wallet wallet = new(userId) { Balance = 500 };
        _walletRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(wallet);
        _walletRepositoryMoq.Setup(r => r.SubtractAsync(It.IsAny<Wallet>(), It.IsAny<decimal>(), token)).Returns(Task.CompletedTask);

        // Act
        var result = await _walletService.SubtractMoneyAsync(wallet.Id, 300, token);

        // Assert
        Assert.True(result);
        _walletRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
        _walletRepositoryMoq.Verify(r => r.SubtractAsync(It.IsAny<Wallet>(), It.IsAny<decimal>(), token));
    }

}