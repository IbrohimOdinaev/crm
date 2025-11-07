namespace crm.BusinessLogic.Dtos;

public sealed record ChangePasswordDto(string Name, string PasswordHash, string NewPassword);