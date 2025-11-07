
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.Enums;
using crm.DataAccess.IRepositories;

namespace crm.DataAccess.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    public Task AddAsync(Order order, CancellationToken token)
    {
        DataStorage.Orders.Add(order);

        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken token)
        => Task.FromResult(DataStorage.Orders.FirstOrDefault(order => order.Id == orderId));
    public Task<IEnumerable<Order>> GetAllAsync(CancellationToken token)
        => Task.FromResult((IEnumerable<Order>)DataStorage.Orders);

    public Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus orderStatus, CancellationToken token)
        => Task.FromResult(DataStorage.Orders.Where(order => order.OrderStatus == orderStatus));

    public Task UpdateAsync(Order order, Order newOrder, CancellationToken token)
    {
        order.DeliveryAddress = newOrder.DeliveryAddress;
        order.Notes = newOrder.Notes;
        order.OrderStatus = newOrder.OrderStatus;
        order.PaymentStatus = newOrder.PaymentStatus;
        order.TotalAmount = newOrder.TotalAmount;
        order.UpdatedAt = newOrder.UpdatedAt;

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Order>> GetAllUserOrdersAsync(Guid userId, CancellationToken token)
        => Task.FromResult(DataStorage.Orders.Where(order => order.UserId == userId));
}