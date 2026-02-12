using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Locations.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;

namespace Application.Entities.Locations.Commands
{
    public record ActivateLocationCommand : IRequest<Either<LocationException, Location>>
    {
        public required Guid LocationId { get; init; }
    }

    public class ActivateLocationCommandHandler(
        ILocationsQueries queries,
        ILocationRepository repository)
        : IRequestHandler<ActivateLocationCommand, Either<LocationException, Location>>
    {
        public async Task<Either<LocationException, Location>> Handle(
            ActivateLocationCommand request,
            CancellationToken cancellationToken)
        {
            var id = new LocationId(request.LocationId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                l => ActivateLocation(l, cancellationToken),
                () => new LocationNotFoundException(id));
        }

        private async Task<Either<LocationException, Location>> ActivateLocation(
            Location entity,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Activate();
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledLocationException(entity.Id, ex);
            }
        }
    }
}