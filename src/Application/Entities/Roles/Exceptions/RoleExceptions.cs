using Domain.Models.Roles;

namespace Application.Entities.Roles.Exceptions
{
    public abstract class RoleException(RoleId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public RoleId LocationId { get; } = id;

    }

    public class RoleNotFoundException(RoleId id)
        : RoleException(id, $"Role with id '{id}' was not found.");

    public class RoleAlreadyExistsException(RoleId id)
        : RoleException(id, $"Role with id '{id}' already exists.");

    public class UnhandledRoleException(RoleId id, Exception? innerException = null)
        : RoleException(id, "Unexpected error occured", innerException);
}
