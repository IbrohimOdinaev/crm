using crm.BusinessLogic.IServices;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Helpers.Extensions;

namespace crm.BusinessLogic.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;

    public OrderItemService(IOrderItemRepository orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }

    public async Task<OrderItemDto?> GetByIdAsync(Guid orderItemId, CancellationToken token)
    {
        OrderItem? orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);

        if (orderItem is null) return null;

        return orderItem.ToOrderItemDto();
    }

    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync(Guid orderId, CancellationToken token)
        => (await _orderItemRepository.GetAllOrderOrderItemsAsync(orderId)).Select(orderItem => orderItem.ToOrderItemDto());

    public async Task UpdateAsync(OrderItemDto orderItemDto, CancellationToken token)
    {
        OrderItem orderItem = (await _orderItemRepository.GetByIdAsync(orderItemDto.Id))!;

        await _orderItemRepository.UpdateAsync(orderItem, orderItemDto.ToDbOrderItem());
    }
}