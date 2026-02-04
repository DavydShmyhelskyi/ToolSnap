using Domain.Models.Locations;

namespace Api.DTOs
{
    public record LocationDto(
        Guid Id,
        string Name,
        Guid LocationTypeId,
        string? Address,
        double Latitude,
        double Longitude,
        bool IsActive,
        DateTime CreatedAt)
    {
        public static LocationDto FromDomain(Location location) =>
            new(
                location.Id.Value,
                location.Name,
                location.LocationTypeId.Value,
                location.Address,
                location.Latitude,
                location.Longitude,
                location.IsActive,
                location.CreatedAt);
    }

    public record CreateLocationDto(
        string Name,
        Guid LocationTypeId,
        string? Address,
        double Latitude,
        double Longitude,
        bool IsActive = true);

    public record UpdateLocationDto(
        string Name,
        Guid LocationTypeId,
        string? Address,
        double Latitude,
        double Longitude);
}