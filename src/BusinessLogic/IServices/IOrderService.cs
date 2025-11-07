namespace crm.BusinessLogic.IServices;

using crm.DataAccess.Entities;
using crm.DataAccess.Enums;
using crm.BusinessLogic.Dtos;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(OrderDto orderDto, CancellationToken token = default);

    Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto, CancellationToken token = default);

    Task<bool> CancelOrderAsync(Guid orderId, CancellationToken token = default);

    Task AddProductToOrderAsync(Guid orderId, Guid productId, int quantity, CancellationToken token = default);

    Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken token = default);

    Task UpdateAsync(OrderDto orderDto, CancellationToken token = default);

    Task<IEnumerable<OrderDto>> GetAllUserOrdersAsync(Guid userId, CancellationToken token = default);

    Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken token = default);

    Task<OrderDto> UpdateDtoDataAsync(Guid orderId, CancellationToken token = default);
}