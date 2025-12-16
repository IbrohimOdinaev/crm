namespace crm.BusinessLogic.Helpers.Extensions;

using crm.DataAccess.Entities;
using crm.BusinessLogic.Dtos;
using crm.DataAccess.Enums;

public static class PaymentExtensions
{
    public static Payment ToDbPayment(this PaymentDto paymentDto)
    => new()
    {
        Id = paymentDto.Id,

        OrderId = paymentDto.OrderId,

        UserId = paymentDto.UserId,

        PaymentStatus = paymentDto.PaymentStatus,

        Amount = paymentDto.Amount
    };

    public static PaymentDto ToPaymentDto(this Payment dbPayment)
    => new()
    {
        Id = dbPayment.Id,

        OrderId = dbPayment.OrderId,

        Amount = dbPayment.Amount,

        UserId = dbPayment.UserId,

        PaymentStatus = dbPayment.PaymentStatus
    };


}