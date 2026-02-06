using Domain.Models.Users;

namespace Api.DTOs
{
    public record UserDto(
        Guid Id,
        string FullName,
        string Email,
        bool ConfirmedEmail,
        Guid RoleId,
        bool IsActive,
        DateTime CreatedAt)
    {
        public static UserDto FromDomain(User user) =>
            new(
                user.Id.Value,
                user.FullName,
                user.Email,
                user.ConfirmedEmail,
                user.RoleId.Value,
                user.IsActive,
                user.CreatedAt);
    }

    public record CreateUserDto(
        string FullName,
        string Email,
        Guid RoleId,
        string Password,
        bool IsActive = true);

    public record UpdateUserDto(
        string FullName,
        string Email,
        Guid RoleId);
}