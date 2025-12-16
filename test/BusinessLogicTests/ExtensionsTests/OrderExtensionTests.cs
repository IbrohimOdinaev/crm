using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;
using System.IO.Pipelines;
using FluentAssertions;
using crm.DataAccess.Enums;
using crm.BusinessLogic.Helpers.Extensions;
using crm.BusinessLogic.Helpers.Extenions;

namespace test.BusinessLogicTests.ExtensionsTests;

public class OrderExtensionTests
{
    [Fact]
    public void ToDbOrder_ConvertsToDbOrder()
    {
        //Arrange
        OrderDto orderDto = new()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            UserId = Guid.NewGuid(),
            DeliveryAddress = "Kulob",
            TotalAmount = 100000,
            PaymentStatus = PaymentStatus.Pending
        };

        //Act
        var result = orderDto.ToDbOrder();

        //Assert
        result.Should().BeOfType<Order>().Which.Should().BeEquivalentTo(orderDto);
    }
    
    [Fact]
    public void ToProductDto_ConvertsToProductDto()
    {
        //Arrange
        Order order = new()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            UserId = Guid.NewGuid(),
            DeliveryAddress = "Kulob",
            TotalAmount = 100000,
            PaymentStatus = PaymentStatus.Pending
        };

        //Act
        var result = order.ToOrderDto();

        //Assert
        result.Should().BeOfType<OrderDto>().Which.Should().BeEquivalentTo(order);
    }
}