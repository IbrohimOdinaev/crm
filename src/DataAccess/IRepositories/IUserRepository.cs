using crm.DataAccess.Entities;

namespace crm.DataAccess.IRepositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken token = default);

    Task<User?> GetByNameAsync(string name, CancellationToken token = default);

    Task AddAsync(User user, CancellationToken token = default);

    Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);

    Task UpdateAsync(User user, User newUser, CancellationToken token = default);
}