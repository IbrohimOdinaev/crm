namespace crm.BusinessLogic.Helpers.Extensions;

using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;

public static class ProductExtension
{
    public static Product ToDbProduct(this ProductDto productDto)
    => new()
    {
        Id = productDto.Id,

        Name = productDto.Name,

        Description = productDto.Description,

        Price = productDto.Price,

        StockQuantity = productDto.StockQuantity,

        Category = productDto.Category,

        IsAvailable = productDto.IsAvailable,

        CreatedAt = productDto.CreatedAt,

        UpdatedAt = productDto.UpdatedAt
    };

    public static ProductDto ToProductDto(this Product dbProduct)
    => new()
    {
        Id = dbProduct.Id,

        Name = dbProduct.Name,

        Description = dbProduct.Description,

        Price = dbProduct.Price,

        StockQuantity = dbProduct.StockQuantity,

        Category = dbProduct.Category,

        IsAvailable = dbProduct.IsAvailable,

        CreatedAt = dbProduct.CreatedAt,

        UpdatedAt = dbProduct.UpdatedAt
    };
}