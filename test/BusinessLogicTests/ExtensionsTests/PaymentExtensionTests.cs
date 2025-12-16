using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;
using crm.BusinessLogic.Helpers.Extensions;
using System.IO.Pipelines;
using FluentAssertions;
using crm.DataAccess.Enums;

namespace test.BusinessLogicTests.ExtensionsTests;

public class PaymentExtensionTests
{
    [Fact]
    public void ToDbPayment_ConvertsToDbPayment()
    {
        //Arrange
        PaymentDto paymentDto = new()
        {
            Id = Guid.NewGuid(),
            Amount = 12,
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentStatus = PaymentStatus.Pending
        };

        //Act
        var result = paymentDto.ToDbPayment();

        //Assert
        result.Should().BeOfType<Payment>().Which.Should().BeEquivalentTo(paymentDto);
    }
    
    [Fact]
    public void ToProductDto_ConvertsToProductDto()
    {
        //Arrange
        Payment payment = new()
        {
            Id = Guid.NewGuid(),
            Amount = 12,
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PaymentStatus = PaymentStatus.Pending
        };

        //Act
        var result = payment.ToPaymentDto();

        //Assert
        result.Should().BeOfType<PaymentDto>().Which.Should().BeEquivalentTo(payment);
    }
}