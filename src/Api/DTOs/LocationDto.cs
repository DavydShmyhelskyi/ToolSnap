using Domain.Models.Locations;

namespace Api.DTOs
{
    public record LocationDto(
        Guid Id,
        string Name,
        Guid LocationTypeId,
        string? LocationTypeName,
        string? Address,
        double? Latitude,
        double? Longitude,
        bool IsActive,
        DateTimeOffset CreatedAt)
    {
        public static LocationDto FromDomain(Location location) =>
            new(
                location.Id.Value,
                location.Name,
                location.LocationTypeId.Value,
                location.LocationType?.Title,
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
        double? Latitude,
        double? Longitude);

    public record UpdateLocationDto(
        string Name,
        Guid LocationTypeId,
        string? Address,
        double? Latitude,
        double? Longitude);
}