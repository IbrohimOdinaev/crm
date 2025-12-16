namespace crm.DataAccess.Entities;

using crm.DataAccess.Enums;

public sealed class Product
{
    public Guid Id { get; init; }

    public decimal Price { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public ProductCategory Category { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }
}