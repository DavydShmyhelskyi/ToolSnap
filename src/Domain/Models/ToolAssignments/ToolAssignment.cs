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
        public LocationId TakenLocationId { get; private set; }
        public LocationId? ReturnedLocationId { get; private set; }
        public DateTime TakenAt { get; }
        public DateTime? ReturnedAt { get; private set; }

        // navigation properties
        public Tool? Tool { get; private set; }
        public User? User { get; private set; }
        public DetectedTool? TakenDetectedTool { get; private set; }
        public DetectedTool? ReturnedDetectedTool { get; private set; }
        public Location? TakenLocation { get; private set; }
        public Location? ReturnedLocation { get; private set; }

        private ToolAssignment(ToolAssignmentId id, DetectedToolId takenDetectedToolId, DetectedToolId? returnedDetectedToolId, ToolId toolId, UserId userId, LocationId takenLocationId, LocationId? returnedLocationId, DateTime takenAt, DateTime? returnedAt)
        {
            Id = id;
            TakenDetectedToolId = takenDetectedToolId;
            ReturnedDetectedToolId = returnedDetectedToolId;
            ToolId = toolId;
            UserId = userId;
            TakenLocationId = takenLocationId;
            ReturnedLocationId = returnedLocationId;
            TakenAt = takenAt;
            ReturnedAt = returnedAt;
        }

        public static ToolAssignment New(DetectedToolId takenDetectedToolId, ToolId toolId, UserId userId, LocationId takenLocationId, DateTimeOffset takenAt)
        {
            return new ToolAssignment(ToolAssignmentId.New(), takenDetectedToolId, null, toolId, userId, takenLocationId, null, DateTime.UtcNow, null);
        }

        public void Return(LocationId returnedLocationId)
        {
            ReturnedLocationId = returnedLocationId;
            ReturnedAt = DateTime.UtcNow;
        }
    }
}