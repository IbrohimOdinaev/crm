namespace crm.BusinessLogic.Dtos;

using crm.DataAccess.Entities;
using crm.DataAccess.Enums;

public sealed record UserDto
{
    private readonly Guid? _id;

    public Guid Id
    {
        get => _id ?? Guid.Empty;
        init => _id = (value == Guid.Empty) ? Guid.NewGuid() : value;
    }

    public string Name {get; init;} = string.Empty;

    public string Email {get; init;} = string.Empty;

    public string PasswordHash {get; init;} = string.Empty;

    public UserRole UserRole {get; init;} 

    public DateTime CreatedAt {get; init;}

    public DateTime LastLoginAt {get; init;}

    public WalletDto? Wallet { get; init; }
    
    public bool IsActive { get; init; }

    public override string ToString()
    {
        return $"{Name} | {Email} | {PasswordHash} | {Wallet!.Balance} | {IsActive} | {Id}";
    }
}