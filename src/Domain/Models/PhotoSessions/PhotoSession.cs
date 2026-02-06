using Domain.Models.DetectedTools;
using Domain.Models.Locations;

namespace Domain.Models.PhotoSessions
{
    public class PhotoSession
    {
        public PhotoSessionId Id { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public ActionTypeId ActionTypeId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        // navigation properties
        public IEnumerable<PhotoForDetection> PhotosForDetection { get; private set; } = new List<PhotoForDetection>();
        public ActionType? ActionType { get; private set; }
        public IEnumerable<DetectedTool> DetectedTools { get; private set; } = new List<DetectedTool>();

        private PhotoSession(PhotoSessionId photoSessionId, double latitude, double longitude, ActionTypeId actionTypeId) 
        {
            Id = photoSessionId;
            Latitude = latitude;
            Longitude = longitude;
            ActionTypeId = actionTypeId;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static PhotoSession New(double latitude, double longitude, ActionTypeId actionTypeId)
        {
            return new PhotoSession(PhotoSessionId.New(), latitude, longitude, actionTypeId);
        }
    }
}
