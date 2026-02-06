using Domain.Models.Roles;

namespace Api.DTOs
{
    public record RoleDto(
        Guid Id,
        string Title)
    {
        public static RoleDto FromDomain(Role role) =>
            new(
                role.Id.Value,
                role.Title);
    }

    public record CreateRoleDto(
        string Title);

    public record UpdateRoleDto(
        string Title);
}
