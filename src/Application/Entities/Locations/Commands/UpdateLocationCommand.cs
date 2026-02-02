using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Locations.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;


namespace Application.Entities.Locations.Commands
{
    public record UpdateLocationCommand : IRequest<Either<LocationException, Location>>
    {
        public required Guid LocationId { get; init; }
        public required string Name { get; init; }
    }

    public class UpdateLocationCommandHandler(
        ILocationsQueries queries,
        ILocationRepository repository)
        : IRequestHandler<UpdateLocationCommand, Either<LocationException, Location>>
    {
        public async Task<Either<LocationException, Location>> Handle(
            UpdateLocationCommand request,
            CancellationToken cancellationToken)
        {
            var id = new LocationId(request.LocationId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                l => UpdateEntity(l, request, cancellationToken),
                () => new LocationNotFoundException(id));
        }


        private async Task<Either<LocationException, Location>> UpdateEntity(
            Location entity,
            UpdateLocationCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
               entity.UpdateName(request.Name);
               return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledLocationException(entity.Id, ex);
            }

        }

    }
}
