using Domain.Models.Tools;

namespace Application.ToolStatuses.Exceptions
{
    public abstract class ToolStatusException(ToolStatusId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ToolStatusId ToolStatusId { get; } = id;
    }

    public class ToolStatusNotFoundException(ToolStatusId id)
        : ToolStatusException(id, $"Tool status with id '{id}' was not found.");

    public class ToolStatusAlreadyExistsException(ToolStatusId id)
        : ToolStatusException(id, $"Tool status with id '{id}' already exists.");

    public class UnhandledToolStatusException(ToolStatusId id, Exception? innerException = null)
        : ToolStatusException(id, "Unexpected error occured", innerException);
}