using Domain.Models.Users;

namespace Api.DTOs
{
    public record UserDto(
        Guid Id,
        string FullName,
        string Email,
        Guid RoleId,
        string? RoleName,
        bool IsActive,
        DateTime CreatedAt)
    {
        public static UserDto FromDomain(User user) =>
            new(
                user.Id.Value,
                user.FullName,
                user.Email,
                user.RoleId.Value,
                user.Role?.Name,
                user.IsActive,
                user.CreatedAt);
    }

    public record CreateUserDto(
        string FullName,
        string Email,
        Guid RoleId,
        string Password);

    public record UpdateUserDto(
        string FullName,
        string Email);

    public record ChangePasswordDto(string NewPassword);
}