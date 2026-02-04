using Domain.Models.Locations;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class LocationControllerService(ILocationsQueries locationsQueries) : ILocationControllerService
    {
        public async Task<Option<LocationDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await locationsQueries.GetByIdAsync(new LocationId(id), cancellationToken);

            return entity.Match(
                l => LocationDto.FromDomain(l),
                () => Option<LocationDto>.None);
        }
    }
}