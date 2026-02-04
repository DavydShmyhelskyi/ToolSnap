using Domain.Models.PhotoSessions;

namespace Application.Entities.PhotoForDetections.Exceptions
{
    public abstract class PhotoForDetectionException(PhotoForDetectionId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public PhotoForDetectionId PhotoForDetectionId { get; } = id;
    }

    public class PhotoForDetectionNotFoundException(PhotoForDetectionId id)
        : PhotoForDetectionException(id, $"Photo for detection with id '{id}' was not found.");

    public class PhotoSessionNotFoundForPhotoException(PhotoSessionId photoSessionId)
        : PhotoForDetectionException(PhotoForDetectionId.Empty(), $"Photo session with id '{photoSessionId}' was not found. Cannot create photo for detection.")
    {
        public PhotoSessionId PhotoSessionId { get; } = photoSessionId;
    }

    public class UnhandledPhotoForDetectionException(PhotoForDetectionId id, Exception? innerException = null)
        : PhotoForDetectionException(id, "Unexpected error occured", innerException);
}