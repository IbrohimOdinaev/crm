namespace crm.BusinessLogic.Helpers.Extensions;

using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;

public static class OrderItemExtensions
{
    public static OrderItem ToDbOrderItem(this OrderItemDto orderItemDto)
    => new()
    {
        Id = orderItemDto.Id,
        
        OrderId = orderItemDto.OrderId,

        ProductId = orderItemDto.ProductId,

        ProductName = orderItemDto.ProductName,

        UnitPrice = orderItemDto.UnitPrice,

        Quantity = orderItemDto.Quantity,

        Subtotal = orderItemDto.Subtotal
    };

    public static OrderItemDto ToOrderItemDto(this OrderItem dbOrderItem)
    => new()
    {
        Id = dbOrderItem.Id,
        
        OrderId = dbOrderItem.OrderId,

        ProductId = dbOrderItem.ProductId,

        ProductName = dbOrderItem.ProductName,

        UnitPrice = dbOrderItem.UnitPrice,

        Quantity = dbOrderItem.Quantity,

        Subtotal = dbOrderItem.Subtotal
    };
}