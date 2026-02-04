using Domain.Models.ToolAssignments;
using Domain.Models.Locations;
using Domain.Models.DetectedTools;
using Domain.Models.Tools;
using Domain.Models.Users;

namespace Application.Entities.ToolAssignments.Exceptions
{
    public abstract class ToolAssignmentException(ToolAssignmentId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ToolAssignmentId ToolAssignmentId { get; } = id;
    }

    public class ToolAssignmentNotFoundException(ToolAssignmentId id)
        : ToolAssignmentException(id, $"Tool assignment with id '{id}' was not found.");

    public class ToolAssignmentAlreadyExistsException(ToolAssignmentId id)
        : ToolAssignmentException(id, $"Tool assignment with id '{id}' already exists.");

    public class UnhandledToolAssignmentException(ToolAssignmentId id, Exception? innerException = null)
        : ToolAssignmentException(id, "Unexpected error occured", innerException);

    public class ToolAssignmentAlreadyReturnedException(ToolAssignmentId id)
        : ToolAssignmentException(id, $"Tool assignment with id '{id}' is already returned.");

    public class CannotUpdateReturnedToolAssignmentException(ToolAssignmentId id)
        : ToolAssignmentException(id, $"Cannot update location for returned tool assignment with id '{id}'.");

    public class LocationNotFoundForToolAssignmentException(LocationId locationId)
        : ToolAssignmentException(ToolAssignmentId.Empty(), $"Location with id '{locationId}' was not found.");

    public class DetectedToolNotFoundForToolAssignmentException(DetectedToolId detectedToolId)
        : ToolAssignmentException(ToolAssignmentId.Empty(), $"Detected tool with id '{detectedToolId}' was not found.");

    public class ToolNotFoundForToolAssignmentException(ToolId toolId)
        : ToolAssignmentException(ToolAssignmentId.Empty(), $"Tool with id '{toolId}' was not found.");

    public class UserNotFoundForToolAssignmentException(UserId userId)
        : ToolAssignmentException(ToolAssignmentId.Empty(), $"User with id '{userId}' was not found.");
}
