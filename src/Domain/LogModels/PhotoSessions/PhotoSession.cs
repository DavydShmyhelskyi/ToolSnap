using Domain.Models.Locations;
using Domain.Models.Users;

namespace Domain.LogModels.PhotoSessions
{

    public class PhotoSession
    {
        public PhotoSessionId Id { get; private set; }
        public UserId UserId { get; private set; }
        public LocationId LocationId { get; private set; }
        public ActionTypeId ActionTypeId { get; private set; }
        public string PhotoUrl { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private PhotoSession() { }

        public static PhotoSession New(UserId userId, LocationId locationId, ActionTypeId actionTypeId, string photoUrl)
        {
            if (userId is null) throw new ArgumentNullException(nameof(userId));
            if (locationId is null) throw new ArgumentNullException(nameof(locationId));
            if (string.IsNullOrWhiteSpace(photoUrl)) throw new ArgumentException("PhotoUrl is required.", nameof(photoUrl));

            return new PhotoSession
            {
                Id = PhotoSessionId.New(),
                UserId = userId,
                LocationId = locationId,
                ActionTypeId = actionTypeId,
                PhotoUrl = photoUrl.Trim(),
                CreatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
