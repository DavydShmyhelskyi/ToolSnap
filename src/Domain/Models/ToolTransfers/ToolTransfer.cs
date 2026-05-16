using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;

namespace Domain.Models.ToolTransfers
{
    public class ToolTransfer
    {
        public ToolTransferId Id { get; }
        public ToolId ToolId { get; }
        public ToolAssignmentId ToolAssignmentId { get; }
        public UserId FromUserId { get; }
        public UserId ToUserId { get; }
        public ToolTransferStatus Status { get; private set; }
        public DateTime InitiatedAt { get; }
        public DateTime? RespondedAt { get; private set; }

        // navigation properties
        public Tool? Tool { get; private set; }
        public ToolAssignment? ToolAssignment { get; private set; }
        public User? FromUser { get; private set; }
        public User? ToUser { get; private set; }

        private ToolTransfer(
            ToolTransferId id,
            ToolId toolId,
            ToolAssignmentId toolAssignmentId,
            UserId fromUserId,
            UserId toUserId,
            ToolTransferStatus status,
            DateTime initiatedAt,
            DateTime? respondedAt)
        {
            Id = id;
            ToolId = toolId;
            ToolAssignmentId = toolAssignmentId;
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Status = status;
            InitiatedAt = initiatedAt;
            RespondedAt = respondedAt;
        }

        public static ToolTransfer New(
            ToolId toolId,
            ToolAssignmentId toolAssignmentId,
            UserId fromUserId,
            UserId toUserId)
        {
            return new ToolTransfer(
                ToolTransferId.New(),
                toolId,
                toolAssignmentId,
                fromUserId,
                toUserId,
                ToolTransferStatus.Pending,
                DateTime.UtcNow,
                null);
        }

        public void Accept()
        {
            Status = ToolTransferStatus.Accepted;
            RespondedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            Status = ToolTransferStatus.Rejected;
            RespondedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            Status = ToolTransferStatus.Cancelled;
            RespondedAt = DateTime.UtcNow;
        }
    }
}
