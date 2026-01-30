using Domain.Enums;

namespace Domain.Models.Locations
{
    public class Location
    {
        public LocationId Id { get; }
        public string Name { get; private set; }
        public LocationType LocationType { get; private set; }
        public string? Address { get; private set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsActive { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        private Location(LocationId id, 
            string name, 
            LocationType locationType, 
            string? address, 
            double latitude, 
            double longitude, 
            bool isActive, 
            DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            LocationType = locationType;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public static Location New(string name, LocationType locationType, string? address, double latitude, double longitude)
        {
            return new Location(
                LocationId.New(),
                name.Trim(),
                locationType,
                string.IsNullOrWhiteSpace(address) ? null : address.Trim(),
                latitude,
                longitude,
                true,
                DateTimeOffset.UtcNow);
        }
        public void Update(string name, LocationType locationType, string? address, double latitude, double longitude)
        {
            Name = name.Trim();
            LocationType = locationType;
            Address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();
            Latitude = latitude;
            Longitude = longitude;
        }
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}