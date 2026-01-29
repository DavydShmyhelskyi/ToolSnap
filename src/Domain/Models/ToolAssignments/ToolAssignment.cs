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
        public DateTimeOffset TakenAt { get; }
        public DateTimeOffset? ReturnedAt { get; private set; }

        private ToolAssignment(ToolAssignmentId id, ToolId toolId, UserId userId, LocationId locationId, DateTimeOffset takenAt, DateTimeOffset? returnedAt)
        {
            Id = id;
            ToolId = toolId;
            UserId = userId;
            LocationId = locationId;
            TakenAt = takenAt;
            ReturnedAt = returnedAt;
        }

        public static ToolAssignment New(ToolId toolId, UserId userId, LocationId locationId, DateTimeOffset takenAt)
        {
            if (toolId is null) throw new ArgumentNullException(nameof(toolId));
            if (userId is null) throw new ArgumentNullException(nameof(userId));
            if (locationId is null) throw new ArgumentNullException(nameof(locationId));

            return new ToolAssignment(ToolAssignmentId.New(), toolId, userId, locationId, takenAt, null);
        }

        public void Return(DateTimeOffset returnedAt)
        {
            if (ReturnedAt.HasValue) throw new InvalidOperationException("Tool already returned.");
            if (returnedAt < TakenAt) throw new ArgumentException("ReturnedAt cannot be earlier than TakenAt.", nameof(returnedAt));

            ReturnedAt = returnedAt;
        }
    }
}