using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;
using crm.BusinessLogic.Helpers.Extensions;
using System.IO.Pipelines;
using FluentAssertions;
using System.Threading.Tasks;

namespace test.BusinessLogicTests.ExtensionsTests;

public class UserExtensionTest
{
    [Fact]
    public async Task ToDbUser_ConvertsToDbUser()
    {
        //Arrange
        UserDto userDto = new()
        {
            Id = Guid.NewGuid(),
            Name = "Siyovush",
            Email = "fuckoff@gmail.com",
            CreatedAt = DateTime.Now,
            IsActive = true,
            PasswordHash = "*******",
        };

        //Act
        var result = await userDto.ToDbUser();

        //Assert
        result.Should().BeOfType<User>().Which.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task ToUserDto_ConvertsToUserDto()
    {
        //Arrange
        User user = new()
        {
            Id = Guid.NewGuid(),
            Name = "Siyovush",
            Email = "fuckoff@gmail.com",
            CreatedAt = DateTime.Now,
            IsActive = true,
            PasswordHash = "*******"
        };
        
        //Act
        var result = user.ToUserDto();

        //Assert
        result.Should().BeOfType<UserDto>().Which.Should().BeEquivalentTo(user);
    }
}