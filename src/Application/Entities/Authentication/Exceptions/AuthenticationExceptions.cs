using Domain.Models.Roles;
using Domain.Models.Users;

namespace Application.Entities.Authentication.Exceptions
{
    public abstract class AuthenticationException(string message, Exception? innerException = null)
        : Exception(message, innerException);

    public class InvalidCredentialsException()
        : AuthenticationException("Invalid email or password.");

    public class EmailAlreadyExistsException(string email)
        : AuthenticationException($"User with email '{email}' already exists.");

    public class UserNotActiveException(UserId userId)
        : AuthenticationException($"User with id '{userId}' is not active.");

    public class RoleNotFoundForAuthenticationException(RoleId roleId)
        : AuthenticationException($"Role with id '{roleId}' was not found.");

    public class DefaultRoleNotFoundException()
        : AuthenticationException("Default 'User' role not found in the system.");

    public class UnhandledAuthenticationException(Exception? innerException = null)
        : AuthenticationException("An unexpected error occurred during authentication.", innerException);
}