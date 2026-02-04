using Domain.Models.Locations;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class LocationTypeControllerService(ILocationTypeQueries locationTypeQueries) : ILocationTypeControllerService
    {
        public async Task<Option<LocationTypeDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await locationTypeQueries.GetByIdAsync(new LocationTypeId(id), cancellationToken);

            return entity.Match(
                l => LocationTypeDto.FromDomain(l),
                () => Option<LocationTypeDto>.None);
        }
    }
}