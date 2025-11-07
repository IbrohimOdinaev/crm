namespace crm.BusinessLogic.Dtos;

using crm.DataAccess.Enums;

public sealed record ProductDto
{
    private readonly Guid? _id;

    public Guid Id
    {
        get => _id ?? Guid.Empty;
        init => _id = (value == Guid.Empty) ? Guid.NewGuid() : value;
    }

    public decimal Price { get; init; }
    public string Name { get; init; } = string.Empty;

    public string Description {get; init;} = string.Empty;

    public int StockQuantity {get; init;}

    public ProductCategory Category {get; init;}

    public bool IsAvailable {get; init;}
    
    public DateTime CreatedAt {get; init;}

    public DateTime UpdatedAt { get; init; }

    public override string ToString()
    {
        return $"{Name} | {Price} | {StockQuantity} | {Category} | {Id}"; 
    }
}