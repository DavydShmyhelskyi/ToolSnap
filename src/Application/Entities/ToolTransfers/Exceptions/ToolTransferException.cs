using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;

namespace Application.Entities.ToolTransfers.Exceptions
{
    public abstract class ToolTransferException(ToolTransferId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ToolTransferId ToolTransferId { get; } = id;
    }

    public class ToolTransferNotFoundException(ToolTransferId id)
        : ToolTransferException(id, $"Tool transfer with id '{id}' was not found.");

    public class ToolTransferNotPendingException(ToolTransferId id)
        : ToolTransferException(id, $"Tool transfer with id '{id}' is not in Pending status.");

    public class ToolTransferUnauthorizedException(ToolTransferId id)
        : ToolTransferException(id, $"You are not authorized to respond to transfer '{id}'.");

    public class ToolTransferSelfTransferException()
        : ToolTransferException(ToolTransferId.Empty(), "Cannot transfer a tool to yourself.");

    public class ToolTransferPendingAlreadyExistsException(ToolId toolId)
        : ToolTransferException(ToolTransferId.Empty(), $"A pending transfer for tool '{toolId}' already exists.");

    public class ToolNotAssignedToUserException(ToolId toolId, UserId userId)
        : ToolTransferException(ToolTransferId.Empty(), $"Tool '{toolId}' is not currently assigned to user '{userId}'.");

    public class UserNotFoundForToolTransferException(UserId userId)
        : ToolTransferException(ToolTransferId.Empty(), $"User with id '{userId}' was not found.");

    public class ToolNotFoundForToolTransferException(ToolId toolId)
        : ToolTransferException(ToolTransferId.Empty(), $"Tool with id '{toolId}' was not found.");

    public class ToolAssignmentNotFoundForToolTransferException(ToolAssignmentId assignmentId)
        : ToolTransferException(ToolTransferId.Empty(), $"Tool assignment with id '{assignmentId}' was not found.");

    public class UnhandledToolTransferException(ToolTransferId id, Exception? innerException = null)
        : ToolTransferException(id, "Unexpected error occurred.", innerException);
}
