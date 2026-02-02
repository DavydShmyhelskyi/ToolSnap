using Domain.Models.Locations;

namespace Api.DTOs
{
    public record LocationTypeDto(Guid Id, string Title)
    {
        public static LocationTypeDto FromDomain(LocationType locationType) =>
            new(
                locationType.Id.Value,
                locationType.Title);
    }

    public record CreateLocationTypeDto(string Title);

    public record UpdateLocationTypeDto(string Title);
}