namespace crm.DataAccess.Entities;

using crm.DataAccess.Enums;
public class Payment
{
    public Guid Id { get; init; }

    public Guid OrderId { get; init; }

    public Guid UserId { get; init; }

    public decimal Amount { get; set; } = 0;

    public PaymentStatus PaymentStatus { get; set; }
}