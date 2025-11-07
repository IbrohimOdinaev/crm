
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using static test.DataAccessTests.Helpers.EntityHelper;

namespace test.DataAccessTests.RepositoriesTests;

public class UserRepositoryTests
{
    IUserRepository _userRepository = new UserRepository();

    [Fact]
    public async Task GetbyIdAsync_ReturnsCorrectUser()
    {
        //Arrange
        User user = (User)CreateDefaultEntity<User>();
        DataStorage.Users.Add(user);

        //Act
        var result = await _userRepository.GetByIdAsync(user.Id);

        //Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsCorrectUser()
    {
        //Arrange
        DataStorage.Users.Clear();
        User user = new() { Name = "Siyovush" };
        DataStorage.Users.Add(user);

        //Act
        var result = await _userRepository.GetByNameAsync("Siyovush");

        //Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task AddAsync_AddsNewUserToDb()
    {
        //Arrange
        User user = (User)CreateDefaultEntity<User>();
        DataStorage.Users.Add(user);

        //Act
        await _userRepository.AddAsync(user);

        //Assert
        Assert.Contains(user, DataStorage.Users);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsListUsers()
    {
        //Arrange
        DataStorage.Users.Clear();
        List<User> users = new();

        for (int i = 0; i < 10; i++)
        {
            users.Add((User)CreateDefaultEntity<User>());
        }

        DataStorage.Users.AddRange(users);

        //Act
        var result = await _userRepository.GetAllAsync();

        //Assert
        Assert.Equal(users, result);
    }
}