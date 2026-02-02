using Domain.Models.Users;

namespace Application.Users.Exceptions
{
    public abstract class UserException(UserId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public UserId UserId { get; } = id;
    }

    public class UserNotFoundException(UserId id)
        : UserException(id, $"User with id '{id}' was not found.");

    public class UserAlreadyExistsException(UserId id)
        : UserException(id, $"User with id '{id}' already exists.");

    public class UnhandledUserException(UserId id, Exception? innerException = null)
        : UserException(id, "Unexpected error occured", innerException);
}