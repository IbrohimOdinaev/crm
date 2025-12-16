using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;

namespace crm.DataAccess.Repositories;


public class OrderItemRepository : IOrderItemRepository
{
    public Task<OrderItem?> GetByIdAsync(Guid orderItemId, CancellationToken token)
        => Task.FromResult(DataStorage.OrderItems.FirstOrDefault(orderItem => orderItem.Id == orderItemId));

    public Task<List<OrderItem>> GetAllOrderOrderItemsAsync(Guid orderId, CancellationToken token)
        => Task.FromResult(DataStorage.OrderItems.Where(orderItem => orderItem.OrderId == orderId).ToList());

    public Task UpdateAsync(OrderItem orderItem, OrderItem newOrderItem, CancellationToken token)
    {
        orderItem.ProductName = newOrderItem.ProductName;
        orderItem.Quantity = newOrderItem.Quantity;
        orderItem.Subtotal = newOrderItem.Subtotal;
        orderItem.UnitPrice = newOrderItem.UnitPrice;

        return Task.CompletedTask;
    }
}