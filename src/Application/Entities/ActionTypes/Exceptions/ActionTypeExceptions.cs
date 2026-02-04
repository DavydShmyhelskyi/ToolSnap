using Domain.Models.PhotoSessions;

namespace Application.Entities.ActionTypes.Exceptions
{
    public abstract class ActionTypeException(ActionTypeId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ActionTypeId ActionTypeId { get; } = id;
    }

    public class ActionTypeNotFoundException(ActionTypeId id)
        : ActionTypeException(id, $"Action type with id '{id}' was not found.");

    public class ActionTypeAlreadyExistsException(ActionTypeId id)
        : ActionTypeException(id, $"Action type with id '{id}' already exists.");

    public class UnhandledActionTypeException(ActionTypeId id, Exception? innerException = null)
        : ActionTypeException(id, "Unexpected error occured", innerException);
}