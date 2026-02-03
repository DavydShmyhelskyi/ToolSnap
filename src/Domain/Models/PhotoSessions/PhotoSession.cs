using Domain.Models.Locations;
using Domain.Models.Users;

namespace Domain.Models.PhotoSessions
{

    public class PhotoSession
    {
        public PhotoSessionId Id { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public ActionTypeId ActionTypeId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private PhotoSession(PhotoSessionId photoSessionId, double latitude, double longitude, ActionTypeId actionTypeId) 
        {
            Id = photoSessionId;
            Latitude = latitude;
            Longitude = longitude;
            ActionTypeId = actionTypeId;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static PhotoSession New(UserId userId, double latitude, double longitude, ActionTypeId actionTypeId)
        {
            return new PhotoSession(PhotoSessionId.New(), latitude, longitude, actionTypeId);
        }
    }
}
