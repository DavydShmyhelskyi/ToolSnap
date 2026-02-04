using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.Tools;
using Domain.Models.Users;

namespace Domain.Models.ToolAssignments
{
    public class ToolAssignment
    {
        public ToolAssignmentId Id { get; }
        public DetectedToolId TakenDetectedToolId { get; set; }
        public DetectedToolId? ReturnedDetectedToolId { get; set; }
        public ToolId ToolId { get; private set; }
        public UserId UserId { get; private set; }
        public LocationId LocationId { get; private set; }
        public DateTime TakenAt { get; }
        public DateTime? ReturnedAt { get; private set; }

        // navigation properties
        private ToolAssignment(ToolAssignmentId id, DetectedToolId takenDetectedToolId, DetectedToolId? returnedDetectedToolId, ToolId toolId, UserId userId, LocationId locationId, DateTime takenAt, DateTime? returnedAt)
        {
            Id = id;
            TakenDetectedToolId = takenDetectedToolId;
            ReturnedDetectedToolId = returnedDetectedToolId;
            ToolId = toolId;
            UserId = userId;
            LocationId = locationId;
            TakenAt = takenAt;
            ReturnedAt = returnedAt;
        }

        public static ToolAssignment New(DetectedToolId takenDetectedToolId, ToolId toolId, UserId userId, LocationId locationId, DateTimeOffset takenAt)
        {
            return new ToolAssignment(ToolAssignmentId.New(), takenDetectedToolId, null, toolId, userId, locationId, DateTime.UtcNow, null);
        }
        //DateTime.UtcNow
        public void Return()
        {
            if (ReturnedAt.HasValue) throw new InvalidOperationException("Tool already returned.");

            ReturnedAt = DateTime.UtcNow;
        }

        public void UpdateLocation(LocationId locationId)
        {
            if (ReturnedAt.HasValue)
                throw new InvalidOperationException("Cannot update location for returned tool.");

            if (locationId == null || locationId == LocationId.Empty())
                throw new ArgumentException("LocationId is required.", nameof(locationId));

            LocationId = locationId;
        }
    }
}