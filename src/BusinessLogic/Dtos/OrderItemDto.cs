namespace crm.BusinessLogic.Dtos;

using crm.DataAccess.Entities;
using crm.DataAccess.Enums;

public sealed record OrderItemDto
{
    private readonly Guid? _id;

    public Guid Id
    {
        get => _id ?? Guid.Empty;
        init => _id = (value == Guid.Empty) ? Guid.NewGuid() : value;
    }

    public Guid OrderId { get; init; }

    public Guid ProductId { get; init; }

    public string ProductName { get; init; } = string.Empty;

    public decimal UnitPrice { get; init; }

    public int Quantity { get; init; }

    public decimal Subtotal { get; init; }

}