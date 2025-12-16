namespace crm.DataAccess.Entities;

using crm.DataAccess.Enums;
public sealed class Order
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }

    public Guid PaymentId { get; set; }

    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public string DeliveryAddress { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }

    public List<Guid> OrderItemIds = new();
}