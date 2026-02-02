using Domain.Models.ToolPhotos;

namespace Application.Entities.PhotoTypes.Exceptions
{
    public abstract class PhotoTypeException(PhotoTypeId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public PhotoTypeId PhotoTypeId { get; } = id;
    }

    public class PhotoTypeNotFoundException(PhotoTypeId id)
        : PhotoTypeException(id, $"Photo type with id '{id}' was not found.");

    public class PhotoTypeAlreadyExistsException(PhotoTypeId id)
        : PhotoTypeException(id, $"Photo type with id '{id}' already exists.");

    public class UnhandledPhotoTypeException(PhotoTypeId id, Exception? innerException = null)
        : PhotoTypeException(id, "Unexpected error occured", innerException);
}