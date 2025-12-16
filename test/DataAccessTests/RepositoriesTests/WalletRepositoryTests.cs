
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;
using static test.DataAccessTests.Helpers.EntityHelper;

namespace test.DataAccessTests.RepositoriesTests;

public class WalletRepositoryTests
{
    IWalletRepository _walletRepository = new WalletRepository();

    [Fact]
    public async Task AddAsync_AddsNewWalletToDb()
    {
        //Arrange
        Wallet wallet = (Wallet)CreateDefaultEntity<Wallet>();

        //Act
        await _walletRepository.AddAsync(wallet);

        //Assert
        Assert.Contains(wallet, DataStorage.Wallets);
    }

    [Fact]
    public async Task GetbyIdAsync_ReturnsCorrectWallet()
    {
        //Arrange
        Guid walletId = Guid.NewGuid();
        Wallet wallet = new() { Id = walletId };
        DataStorage.Wallets.Add(wallet);

        //Act
        var result = await _walletRepository.GetByIdAsync(walletId);

        //Assert
        Assert.Equal(wallet, result);
    }

    [Fact]
    public async Task GetbyUserIdAsync_ReturnsCorrectWallet()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        Wallet wallet = new() { UserId = userId };
        DataStorage.Wallets.Add(wallet);

        //Act
        var result = await _walletRepository.GetByUserIdAsync(userId);

        //Assert
        Assert.Equal(wallet, result);
    }

    [Fact]
    public async Task TopUpAsync_AddsMoneyToWallet()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        decimal walletBalance = 100;
        Wallet wallet = new(Guid.NewGuid());
        wallet.Balance = walletBalance;
        decimal amount = 223;

        // Arrange
        await _walletRepository.TopUpAsync(wallet, amount, token);

        // Assert
        Assert.Equal(walletBalance + amount, wallet.Balance);
    }

    [Fact]
    public async Task SubtactAsync_SubtractsMoneyFromWallet()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        decimal walletBalance = 400;
        Wallet wallet = new(Guid.NewGuid());
        wallet.Balance = walletBalance;
        decimal amount = 223;

        // Arrange
        await _walletRepository.SubtractAsync(wallet, amount, token);

        // Assert
        Assert.Equal(walletBalance - amount, wallet.Balance);
    }
}