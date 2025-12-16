using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;
using crm.BusinessLogic.Helpers.Extensions;
using System.IO.Pipelines;
using FluentAssertions;
using crm.DataAccess.Enums;

namespace test.BusinessLogicTests.ExtensionsTests;

public class OrderItemExtensionTests
{
    [Fact]
    public void ToDbOrderItem_ReturnsDbOrderItem()
    {
        //Arrange
        OrderItemDto orderItemDto = new()
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Shakarov",
            Quantity = 12,
            Subtotal = 12 * 20,
            UnitPrice = 20
        };

        //Act
        var result = orderItemDto.ToDbOrderItem();

        //Assert
        result.Should().BeOfType<OrderItem>().Which.Should().BeEquivalentTo(orderItemDto);
    }
    
    [Fact]
    public void ToOrderItemDto_ConvertsToOrderItemDto()
    {
        //Arrange
        OrderItem orderItem = new()
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Shakarov",
            Quantity = 12,
            Subtotal = 12 * 20,
            UnitPrice = 20
        };

        //Act
        var result = orderItem.ToOrderItemDto();

        //Assert
        result.Should().BeOfType<OrderItemDto>().Which.Should().BeEquivalentTo(orderItem);
    }
}