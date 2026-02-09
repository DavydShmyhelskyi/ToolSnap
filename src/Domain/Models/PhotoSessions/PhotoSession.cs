using Domain.Models.DetectedTools;
using Domain.Models.Locations;

namespace Domain.Models.PhotoSessions
{
    public class PhotoSession
    {
        public PhotoSessionId Id { get; init; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ActionTypeId ActionTypeId { get; set; }
        public DateTime CreatedAt { get; set; }

        // navigation properties
        public IEnumerable<PhotoForDetection> PhotosForDetection { get; private set; } = new List<PhotoForDetection>();
        public ActionType? ActionType { get; private set; }
        public IEnumerable<DetectedTool> DetectedTools { get; private set; } = new List<DetectedTool>();

        private PhotoSession(PhotoSessionId photoSessionId, double latitude, double longitude, ActionTypeId actionTypeId, DateTime createdAt) 
        {
            Id = photoSessionId;
            Latitude = latitude;
            Longitude = longitude;
            ActionTypeId = actionTypeId;
            CreatedAt = createdAt;
        }

        public static PhotoSession New(double latitude, double longitude, ActionTypeId actionTypeId)
        {
            return new PhotoSession(PhotoSessionId.New(), latitude, longitude, actionTypeId, DateTime.UtcNow);
        }
    }
}
