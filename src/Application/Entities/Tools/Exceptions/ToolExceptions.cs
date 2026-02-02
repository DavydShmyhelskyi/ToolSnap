using Domain.Models.Tools;

namespace Application.Entities.Tools.Exceptions
{
    public abstract class ToolException(ToolId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ToolId ToolId { get; } = id;
    }

    public class ToolNotFoundException(ToolId id)
        : ToolException(id, $"Tool with id '{id}' was not found.");

    public class ToolAlreadyExistsException(ToolId id)
        : ToolException(id, $"Tool with id '{id}' already exists.");

    public class UnhandledToolException(ToolId id, Exception? innerException = null)
        : ToolException(id, "Unexpected error occured", innerException);
}