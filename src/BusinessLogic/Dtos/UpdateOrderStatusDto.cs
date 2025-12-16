using crm.DataAccess.Enums;

namespace crm.BusinessLogic.Dtos;

public record UpdateOrderStatusDto(Guid OrderId, OrderStatus NewOrderStatus);