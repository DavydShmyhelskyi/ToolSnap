using Domain.Models.Locations;
using Domain.Models.Tools;
using Domain.Models.Users;

namespace Domain.Models.ToolAssignments
{
    public class ToolAssignment
    {
        public ToolAssignmentId Id { get; }
        public ToolId ToolId { get; private set; }
        public UserId UserId { get; private set; }
        public LocationId LocationId { get; private set; }
        public DateTime TakenAt { get; }
        public DateTime? ReturnedAt { get; private set; }

        private ToolAssignment(ToolAssignmentId id, ToolId toolId, UserId userId, LocationId locationId)
        {
            Id = id;
            ToolId = toolId;
            UserId = userId;
            LocationId = locationId;
            TakenAt = DateTime.UtcNow;
            ReturnedAt = null;
        }

        public static ToolAssignment New(ToolId toolId, UserId userId, LocationId locationId, DateTimeOffset takenAt)
        {
            return new ToolAssignment(ToolAssignmentId.New(), toolId, userId, locationId);
        }

        public void Return()
        {
            if (ReturnedAt.HasValue) throw new InvalidOperationException("Tool already returned.");

            ReturnedAt = DateTime.UtcNow;
        }
    }
}