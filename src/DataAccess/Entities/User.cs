namespace crm.DataAccess.Entities;

using crm.DataAccess.Enums;
public sealed class User
{
    public Guid Id {get; init;} 

    public string Name {get; set;} = string.Empty;

    public string Email {get; set;} = string.Empty;

    public string PasswordHash {get; set;} = string.Empty;

    public UserRole UserRole { get; set; }

    public DateTime CreatedAt {get; init;}

    public DateTime LastLoginAt {get; set;}

    public Wallet? Wallet { get; init; }

    public bool IsActive { get; set; }
}