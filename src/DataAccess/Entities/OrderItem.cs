namespace crm.DataAccess.Entities;

using crm.DataAccess.Enums;

public sealed class OrderItem
{
    public Guid Id { get; init; }

    public Guid OrderId { get; init; }

    public Guid ProductId { get; init; }

    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Subtotal { get; set; }
}