using System.Runtime.CompilerServices;

namespace Core.DTOs.Requests;

public record CreateUserRequest(
    string UserName,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email
    );

public record UpdateUserRequest(
    string UserName,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email
    );

public class VerifyRequest
{
    public string UserName { get; set; } = default!;
}

public class ChangePasswordRequest
{
    public string UserName { get; set; } = default!;
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}

public class ResetPasswordRequest
{
    public string UserName { get; set; } = default!;
}

public record GetUsersWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Keywords);