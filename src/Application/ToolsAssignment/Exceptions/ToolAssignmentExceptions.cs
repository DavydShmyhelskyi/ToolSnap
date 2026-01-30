using Domain.Models.ToolAssignments;

namespace Application.ToolsAssignment.Exceptions
{
    public class ToolAssignmentExceptions
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
    }
}
