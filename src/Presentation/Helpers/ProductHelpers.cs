using crm.BusinessLogic.Dtos;
using crm.DataAccess.Enums;
using static crm.Presentation.Helpers.BaseHelpers;


namespace crm.Presentation.Helpers;

public static class ProductHelpers
{
    public static async Task<ProductDto> GetProductDtoAsync(CancellationToken token = default)
    {
        decimal price = (await GetStringAsync<decimal>("Price", token));
        
        string name = (await GetStringAsync<string>("Name", token))!;

        string description = (await GetStringAsync<string>("Description", token))!;

        int stockQuantity = await GetStringAsync<int>("Quantity", token);

        ProductCategory productCategory = await GetStringAsync<ProductCategory>("Product cagegory", token);

        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            Category = productCategory,
            IsAvailable = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public static async Task<UpdateStockDto> GetProductStockDtoAsync(CancellationToken token = default)
    {
        Guid id = await GetEntityId("Product", token);

        int newStockQuantity = await GetStringAsync<int>("New quantity", token);

        return new(id, newStockQuantity);
    }


}