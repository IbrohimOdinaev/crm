using crm.DataAccess.Entities;
using crm.DataAccess.DataBase;
namespace crm.DataAccess.IRepositories;


public sealed class UserRepository : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken token)
        => Task.FromResult(DataStorage.Users.FirstOrDefault(user => user.Id == userId));

    public Task<User?> GetByNameAsync(string name, CancellationToken token)
        => Task.FromResult(DataStorage.Users.FirstOrDefault(user => user.Name == name));

    public Task AddAsync(User user, CancellationToken token)
    {
        DataStorage.Users.Add(user);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken token) => Task.FromResult((IEnumerable<User>)DataStorage.Users);

    public Task UpdateAsync(User user, User newUser, CancellationToken token)
    {
        user.Name = newUser.Name;
        user.Email = newUser.Email;
        user.PasswordHash = newUser.PasswordHash;
        user.UserRole = newUser.UserRole;
        user.LastLoginAt = newUser.LastLoginAt;
        user.IsActive = newUser.IsActive;

        return Task.CompletedTask;
    }
}