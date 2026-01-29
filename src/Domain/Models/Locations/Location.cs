using Domain.Enums;

namespace Domain.Models.Locations
{
    public class Location
    {
        public LocationId Id { get; }
        public string Name { get; private set; }
        public LocationType LocationType { get; private set; }
        public string? Address { get; private set; }
        public bool IsActive { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        private Location(LocationId id, string name, LocationType locationType, string? address, bool isActive, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            LocationType = locationType;
            Address = address;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public static Location New(string name, LocationType locationType, string? address = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));

            return new Location(
                LocationId.New(),
                name.Trim(),
                locationType,
                string.IsNullOrWhiteSpace(address) ? null : address.Trim(),
                true,
                DateTimeOffset.UtcNow);
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}