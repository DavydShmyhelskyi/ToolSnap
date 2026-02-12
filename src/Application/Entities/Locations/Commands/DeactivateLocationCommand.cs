using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Locations.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;

namespace Application.Entities.Locations.Commands
{
    public record DeactivateLocationCommand : IRequest<Either<LocationException, Location>>
    {
        public required Guid LocationId { get; init; }
    }

    public class DeactivateLocationCommandHandler(
        ILocationsQueries queries,
        ILocationRepository repository)
        : IRequestHandler<DeactivateLocationCommand, Either<LocationException, Location>>
    {
        public async Task<Either<LocationException, Location>> Handle(
            DeactivateLocationCommand request,
            CancellationToken cancellationToken)
        {
            var id = new LocationId(request.LocationId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                l => DeactivateLocation(l, cancellationToken),
                () => new LocationNotFoundException(id));
        }

        private async Task<Either<LocationException, Location>> DeactivateLocation(
            Location entity,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Deactivate();
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledLocationException(entity.Id, ex);
            }
        }
    }
}