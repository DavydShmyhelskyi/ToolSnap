using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.LocationTypes.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;

namespace Application.Entities.LocationTypes.Commands
{
    public record UpdateLocationTypeCommand : IRequest<Either<LocationTypeException, LocationType>>
    {
        public required Guid LocationTypeId { get; init; }
        public required string Title { get; init; }
    }

    public class UpdateLocationTypeCommandHandler(
        ILocationTypeQueries queries,
        ILocationTypeRepository repository)
        : IRequestHandler<UpdateLocationTypeCommand, Either<LocationTypeException, LocationType>>
    {
        public async Task<Either<LocationTypeException, LocationType>> Handle(
            UpdateLocationTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new LocationTypeId(request.LocationTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                lt => UpdateEntity(lt, request, cancellationToken),
                () => new LocationTypeNotFoundException(id));
        }

        private async Task<Either<LocationTypeException, LocationType>> UpdateEntity(
            LocationType entity,
            UpdateLocationTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.ChangeTitle(request.Title);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledLocationTypeException(entity.Id, ex);
            }
        }
    }
}