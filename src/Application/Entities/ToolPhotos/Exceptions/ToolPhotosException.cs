using Domain.Models.PhotoSessions;
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

    public class UnhandledToolPhotoException(ToolPhotoId id, Exception? innerException = null)
        : ToolPhotoException(id, "Unexpected error occured", innerException);

    public class ToolPhotoNotFoundForToolException(ToolPhotoId id)
        : ToolPhotoException(id, $"Tool photo with id '{id}' was not found for tool with this id.");

    public class ToolNotFoundForToolPhotoException(ToolId toolId)
        : ToolPhotoException(ToolPhotoId.Empty(), $"Tool with id '{toolId}' was not found. Cannot create photo session.");
}