using Domain.Models.PhotoSessions;
using Domain.Models.Tools;
using Domain.Models.Users;

namespace Application.Entities.PhotoSessions.Exceptions
{
    public abstract class PhotoSessionException(PhotoSessionId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public PhotoSessionId PhotoSessionId { get; } = id;
    }

    public class PhotoSessionNotFoundException(PhotoSessionId id)
        : PhotoSessionException(id, $"Photo session with id '{id}' was not found.");

    public class UserNotFoundForPhotoSessionException(UserId userId)
        : PhotoSessionException(PhotoSessionId.Empty(), $"User with id '{userId}' was not found. Cannot create photo session.")
    {
        public UserId UserId { get; } = userId;
    }

    public class ActionTypeNotFoundForPhotoSessionException(ActionTypeId actionTypeId)
        : PhotoSessionException(PhotoSessionId.Empty(), $"Action type with id '{actionTypeId}' was not found. Cannot create photo session.")
    {
        public ActionTypeId ActionTypeId { get; } = actionTypeId;
    }

    public class UnhandledPhotoSessionException(PhotoSessionId id, Exception? innerException = null)
        : PhotoSessionException(id, "Unexpected error occured", innerException);

    }