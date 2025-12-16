namespace crm.BusinessLogic.Dtos;

using crm.DataAccess.Enums;
public sealed record PaymentDto
{
    private readonly Guid? _id;

    public Guid Id
    {
        get => _id ?? Guid.Empty;
        init => _id = (value == Guid.Empty ? Guid.NewGuid() : value);
    }

    public decimal Amount { get; init; } = 0;
    public Guid OrderId { get; init; }

    public Guid UserId { get; init; }

    public PaymentStatus PaymentStatus { get; init; }
}