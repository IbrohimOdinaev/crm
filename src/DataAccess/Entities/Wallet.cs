
namespace crm.DataAccess.Entities;

public sealed class Wallet
{
    public Guid Id { get; set; }

    public decimal Balance { get; set; } = 0;

    public Guid UserId { get; init; }

    public Wallet(Guid userId)
    {
        UserId = userId;
    }
    public Wallet() { }
}