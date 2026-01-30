using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Locations.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;


namespace Application.Locations.Commands
{
    public record CreateLocationCommand : IRequest<Either<LocationException, Location>>
    {
        public required string Name { get; init; }
        public required LocationType LocationType { get; init; }
        public required string? Address { get; init; }
        public required double? Latitude { get; init; }
        public required double? Longitude { get; init; }
        public required bool IsActive { get; init; }
        public required DateTimeOffset CreatedAt { get; init; }
    }

    public class CreateLocationCommandHandler(
        ILocationRepository locationRepository)
        : IRequestHandler<CreateLocationCommand, Either<LocationException, Location>>
    {
        public async Task<Either<LocationException, Location>> Handle(
            CreateLocationCommand request, 
            CancellationToken cancellationToken)
        {
            var existing = await locationRepository.GetByNameAsync(request.Name, cancellationToken);

            return await existing.MatchAsync(
                l => new LocationAlreadyExistsException(l.Id),
                () => CreateEntity(request, cancellationToken));
        }


        private async Task<Either<LocationException, Location>> CreateEntity(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newLocation = Location.New(
               request.Name,
               request.LocationType,
               request.Address,
               request.Latitude ?? 0,
               request.Longitude ?? 0);

                var result = await locationRepository.AddAsync(newLocation, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledLocationException(LocationId.Empty(), ex);
            }

        }

    }
}
