using Domain.Models.ToolInfo;

namespace Application.Entities.ToolTypes.Exceptions
{
    public abstract class ToolTypeException(ToolTypeId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ToolTypeId ToolTypeId { get; } = id;
    }

    public class ToolTypeNotFoundException(ToolTypeId id)
        : ToolTypeException(id, $"Tool type with id '{id}' was not found.");

    public class ToolTypeAlreadyExistsException(ToolTypeId id)
        : ToolTypeException(id, $"Tool type with id '{id}' already exists.");

    public class UnhandledToolTypeException(ToolTypeId id, Exception? innerException = null)
        : ToolTypeException(id, "Unexpected error occured", innerException);
}