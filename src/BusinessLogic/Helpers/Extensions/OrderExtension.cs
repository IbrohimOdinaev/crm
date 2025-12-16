namespace crm.BusinessLogic.Helpers.Extenions;

using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;

public static class OrderExtension
{
    public static Order ToDbOrder(this OrderDto orderDto)
    => new()
    {
        Id = orderDto.Id,

        UserId = orderDto.UserId,

        PaymentId = orderDto.PaymentId,

        TotalAmount = orderDto.TotalAmount,

        PaymentStatus = orderDto.PaymentStatus,

        OrderStatus = orderDto.OrderStatus,

        DeliveryAddress = orderDto.DeliveryAddress,

        Notes = orderDto.Notes,

        CreatedAt = orderDto.CreatedAt,

        UpdatedAt = orderDto.UpdatedAt
    };

    public static OrderDto ToOrderDto(this Order dbOrder)
    => new()
    {
        Id = dbOrder.Id,

        UserId = dbOrder.UserId,

        TotalAmount = dbOrder.TotalAmount,

        PaymentId = dbOrder.PaymentId,

        OrderStatus = dbOrder.OrderStatus,

        PaymentStatus = dbOrder.PaymentStatus,

        DeliveryAddress = dbOrder.DeliveryAddress,

        Notes = dbOrder.Notes,

        CreatedAt = dbOrder.CreatedAt,

        UpdatedAt = dbOrder.UpdatedAt,

        OrderItemIds = dbOrder.OrderItemIds
    };
}