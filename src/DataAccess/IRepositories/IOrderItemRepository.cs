
using crm.DataAccess.Entities;

namespace crm.DataAccess.IRepositories;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetByIdAsync(Guid orderItemId, CancellationToken token = default);

    Task<List<OrderItem>> GetAllOrderOrderItemsAsync(Guid orderId, CancellationToken token = default);

    Task UpdateAsync(OrderItem orderItem, OrderItem newOrderItem, CancellationToken token = default);
}