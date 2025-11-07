using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;

namespace crm.BusinessLogic.IServices;

public interface IOrderItemService
{
    Task<OrderItemDto?> GetByIdAsync(Guid orderItemId, CancellationToken token = default);

    Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync(Guid orderId, CancellationToken token = default);

    Task UpdateAsync(OrderItemDto orderItemdDto, CancellationToken token = default);

}