namespace Api.DTOs;

public record LoginDto(
    string Email,
    string Password);

public record RegisterDto(
    string FullName,
    string Email,
    string Password,
    Guid? RoleId = null);

public record AuthenticationResponseDto(
    Guid Id,
    string FullName,
    string Email,
    string Role,
    bool IsActive,
    bool EmailConfirmed,
    string Token);