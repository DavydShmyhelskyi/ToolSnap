using Domain.Models.ToolAssignments;

namespace Domain.Models.Locations
{
    public class Location
    {
        public LocationId Id { get; }
        public string Name { get; private set; }
        public LocationTypeId LocationTypeId { get; private set; }
        public string? Address { get; private set; }
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }
        public bool IsActive { get; private set; } = false;
        public DateTimeOffset CreatedAt { get; }

        // navigation properties
        public LocationType? LocationType { get; private set; }

        public IReadOnlyCollection<ToolAssignment> ToolAssignments => _assignments;
        private readonly List<ToolAssignment> _assignments = new();
        
        private Location(LocationId id, 
            string name, 
            LocationTypeId locationTypeId, 
            string? address, 
            double? latitude, 
            double? longitude, 
            bool isActive, 
            DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            LocationTypeId = locationTypeId;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public static Location New(string name, LocationTypeId locationTypeId, string? address, double? latitude, double? longitude)
        {
            return new Location(
                LocationId.New(),
                name.Trim(),
                locationTypeId,
                string.IsNullOrWhiteSpace(address) ? null : address.Trim(),
                latitude,
                longitude,
                true,
                DateTimeOffset.UtcNow);
        }
        
        public void Update(string name, LocationTypeId locationTypeId, string? address, double? latitude, double? longitude)
        {
            Name = name.Trim();
            LocationTypeId = locationTypeId;
            Address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();
            Latitude = latitude;
            Longitude = longitude;
        }
        
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void UpdateName(string name)
        {
            Name = name.Trim();
        }
    }
}