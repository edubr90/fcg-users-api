using Users.Domain.Enums;

namespace Users.Application.DTOs;

public record RegisterUserRequest(string Name, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record UpdateUserRequest(string? Name, string? Email);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAt
    );

public record AuthResponse(string Token, UserResponse User);
