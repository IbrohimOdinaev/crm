
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.Enums;

namespace crm.DataAccess.IRepositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken token = default);

    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken token = default);

    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus orderStatus, CancellationToken token = default);

    Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);

    Task UpdateAsync(Order order, Order newOrder, CancellationToken token = default);

    Task<IEnumerable<Order>> GetAllUserOrdersAsync(Guid userId, CancellationToken token = default);
}