using Application.Locations.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;
using Application.Common.Interfaces.Repositories;


namespace Application.Locations.Commands
{
    public record DeleteLocationCommand : IRequest<Either<LocationException, Location>>
    {
        public required Guid LocationId { get; init; }
    }


    public class DeleteLocationCommandHandler(
        ILocationRepository repository)
        : IRequestHandler<DeleteLocationCommand, Either<LocationException, Location>>
    {
        public async Task<Either<LocationException, Location>> Handle(
            DeleteLocationCommand request,
            CancellationToken cancellationToken)
        {
            var id = new LocationId(request.LocationId);
            var entity = await repository.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<LocationException, Location>>(
                l => repository.DeleteAsync(l, cancellationToken).Result,
                () => new LocationNotFoundException(id));
        } 
    }
}