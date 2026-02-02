using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.LocationTypes.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;

namespace Application.LocationTypes.Commands
{
    public record DeleteLocationTypeCommand : IRequest<Either<LocationTypeException, LocationType>>
    {
        public required Guid LocationTypeId { get; init; }
    }

    public class DeleteLocationTypeCommandHandler(
        ILocationTypeQueries queries,
        ILocationTypeRepository repository)
        : IRequestHandler<DeleteLocationTypeCommand, Either<LocationTypeException, LocationType>>
    {
        public async Task<Either<LocationTypeException, LocationType>> Handle(
            DeleteLocationTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new LocationTypeId(request.LocationTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<LocationTypeException, LocationType>>(
                lt => repository.DeleteAsync(lt, cancellationToken).Result,
                () => new LocationTypeNotFoundException(id));
        }
    }
}