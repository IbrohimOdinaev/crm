namespace crm.Presentation.Helpers;

using crm.BusinessLogic.Dtos;
using crm.DataAccess.Enums;
using static crm.Presentation.Helpers.BaseHelpers;

public static class OrderHelpers
{
    public static async Task<OrderDto> GetOrderDtoAsync(Guid userId, CancellationToken token = default)
    {;
        string deliveryAddress = (await GetStringAsync<string>("Delivery address", token))!;

        string notes = (await GetStringAsync<string>("Notes", token))!;

        return new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OrderStatus = OrderStatus.Pending,
            DeliveryAddress = deliveryAddress,
            Notes = notes,
            CreatedAt = DateTime.Now,
            TotalAmount = 0,
            PaymentStatus = PaymentStatus.Pending,
            UpdatedAt = DateTime.Now,
        };
    }
}