using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;

namespace Domain.Models.ToolLiabilities
{
    public class ToolLiability
    {
        public ToolLiabilityId Id { get; }
        public ToolId ToolId { get; }
        public ToolAssignmentId ToolAssignmentId { get; }
        public UserId UserId { get; }
        public decimal PriceAtAssignment { get; }
        public DateTime AssignedAt { get; }
        public DateTime? ClosedAt { get; private set; }

        // computed — not persisted
        public bool IsOpen => ClosedAt == null;

        // navigation properties
        public Tool? Tool { get; private set; }
        public ToolAssignment? ToolAssignment { get; private set; }
        public User? User { get; private set; }

        private ToolLiability(
            ToolLiabilityId id,
            ToolId toolId,
            ToolAssignmentId toolAssignmentId,
            UserId userId,
            decimal priceAtAssignment,
            DateTime assignedAt,
            DateTime? closedAt)
        {
            Id = id;
            ToolId = toolId;
            ToolAssignmentId = toolAssignmentId;
            UserId = userId;
            PriceAtAssignment = priceAtAssignment;
            AssignedAt = assignedAt;
            ClosedAt = closedAt;
        }

        public static ToolLiability New(
            ToolId toolId,
            ToolAssignmentId toolAssignmentId,
            UserId userId,
            decimal priceAtAssignment,
            DateTime assignedAt)
        {
            return new ToolLiability(
                ToolLiabilityId.New(),
                toolId,
                toolAssignmentId,
                userId,
                priceAtAssignment,
                assignedAt,
                null);
        }

        public void Close()
        {
            ClosedAt = DateTime.UtcNow;
        }
    }
}
