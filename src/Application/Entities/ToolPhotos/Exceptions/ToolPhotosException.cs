using Domain.Models.ToolPhotos;
using Domain.Models.Tools;

namespace Application.Entities.ToolPhotos.Exceptions
{
    public abstract class ToolPhotoException(ToolPhotoId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ToolPhotoId ToolPhotoId { get; } = id;
    }

    public class ToolPhotoNotFoundException(ToolPhotoId id)
        : ToolPhotoException(id, $"Tool photo with id '{id}' was not found.");

    public class ToolPhotoAlreadyExistsException(ToolPhotoId id)
        : ToolPhotoException(id, $"Tool photo with id '{id}' already exists.");

    public class ToolNotFoundForToolPhotoException(ToolId toolId)
        : ToolPhotoException(ToolPhotoId.Empty(), $"Tool with id '{toolId}' was not found.");

    public class PhotoTypeNotFoundForToolPhotoException(PhotoTypeId photoTypeId)
        : ToolPhotoException(ToolPhotoId.Empty(), $"Photo type with id '{photoTypeId}' was not found.");

    public class UnhandledToolPhotoException(ToolPhotoId id, Exception? innerException = null)
        : ToolPhotoException(id, "Unexpected error occured", innerException);
}