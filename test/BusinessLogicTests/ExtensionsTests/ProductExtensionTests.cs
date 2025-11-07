using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;
using crm.BusinessLogic.Helpers.Extensions;
using System.IO.Pipelines;
using FluentAssertions;
using crm.DataAccess.Enums;

namespace test.BusinessLogicTests.ExtensionsTests;

public class ProductExtensionTests
{
    [Fact]
    public void ToDbProduct_ConvertsToDbProduct()
    {
        //Arrange
        ProductDto productDto = new()
        {
            Id = Guid.NewGuid(),
            Name = "Kandchoy",
            IsAvailable = true,
            CreatedAt = DateTime.Now,
            StockQuantity = 100,
            Category = ProductCategory.Food
        };

        //Act
        var result = productDto.ToDbProduct();

        //Assert
        result.Should().BeOfType<Product>().Which.Should().BeEquivalentTo(productDto);
    }
    
    [Fact]
    public void ToProductDto_ConvertsToProductDto()
    {
        //Arrange
        Product product = new()
        {
            Id = Guid.NewGuid(),
            Name = "Kandchoy",
            IsAvailable = true,
            CreatedAt = DateTime.Now,
            StockQuantity = 100, 
            Category = ProductCategory.Food
        };

        //Act
        var result = product.ToProductDto();

        //Assert
        result.Should().BeOfType<ProductDto>().Which.Should().BeEquivalentTo(product);
    }
}