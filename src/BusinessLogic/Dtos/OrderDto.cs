namespace crm.BusinessLogic.Dtos;

using crm.DataAccess.Enums;

public sealed record OrderDto
{
    private readonly Guid? _id;
    public Guid Id
    {
        get => _id ?? Guid.Empty;
        init => _id = (value == Guid.Empty) ? Guid.NewGuid() : value;
    }
    public Guid UserId { get; init; }
    public Guid PaymentId { get; set; }

    public decimal TotalAmount { get; init; }

    public OrderStatus OrderStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public string DeliveryAddress { get; init; } = string.Empty;

    public string Notes { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public List<Guid> OrderItemIds = new();

    public override string ToString()
    {
        return $"{UserId} | {DeliveryAddress} | {TotalAmount} | {OrderItemIds.Count} | {OrderStatus} | {PaymentStatus} | {Id}"; 
    }
}