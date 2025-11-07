
using crm.DataAccess.Entities;

namespace crm.BusinessLogic.Dtos;

public record WalletDto
{
    private readonly Guid? _id;

    public Guid Id
    {
        get => _id ?? Guid.Empty;
        init => _id = (value == Guid.Empty) ? Guid.NewGuid() : value;
    }

    public decimal Balance { get; init; }

    public Guid UserId { get; init; }

    public WalletDto(Guid id, Guid userId)
    {
        UserId = userId;
        Id = id;
    }

    public WalletDto() {}
}