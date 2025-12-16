using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;
using crm.BusinessLogic.Helpers.Extensions;
using System.IO.Pipelines;
using FluentAssertions;
using crm.DataAccess.Enums;
using crm.BusinessLogic.Extensions;

namespace test.BusinessLogicTests.ExtensionsTests;

public class WalletExtensionTests
{
    [Fact]
    public void ToDbWallet_ConvertsToDbWallet()
    {
        //Arrange
        WalletDto walletDto = new()
        {
            Id = Guid.NewGuid(),
            Balance = 1000000,
            UserId = Guid.NewGuid()
        };

        //Act
        var result = walletDto.ToDbWallet();

        //Assert
        result.Should().BeOfType<Wallet>().Which.Should().BeEquivalentTo(walletDto);
    }
    
    [Fact]
    public void ToWalletDto_ConvertsToWalletDto()
    {
        //Arrange
        Wallet wallet = new()
        {
            Id = Guid.NewGuid(),
            Balance = 1000000,
            UserId = Guid.NewGuid()
        };

        //Act
        var result = wallet.ToWalletDto();

        //Assert
        result.Should().BeOfType<WalletDto>().Which.Should().BeEquivalentTo(wallet);
    }
}