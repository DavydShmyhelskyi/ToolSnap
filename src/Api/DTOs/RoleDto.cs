using Domain.Models.Roles;

namespace Api.DTOs
{
    public record RoleDto(Guid Id, string Name, DateTimeOffset CreatedAt)
    {
        public static RoleDto FromDomain(Role role) =>
            new(
                role.Id.Value,
                role.Name,
                role.CreatedAt);
    }

    public record CreateRoleDto(string Name);

    public record UpdateRoleDto(Guid Id, string Name);
}
