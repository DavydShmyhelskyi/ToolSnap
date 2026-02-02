using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.LocationTypes.Exceptions;
using Domain.Models.Locations;
using LanguageExt;
using MediatR;

namespace Application.LocationTypes.Commands
{
    public record CreateLocationTypeCommand : IRequest<Either<LocationTypeException, LocationType>>
    {
        public required string Title { get; init; }
    }

    public class CreateLocationTypeCommandHandler(
        ILocationTypeQueries queries,
        ILocationTypeRepository repository)
        : IRequestHandler<CreateLocationTypeCommand, Either<LocationTypeException, LocationType>>
    {
        public async Task<Either<LocationTypeException, LocationType>> Handle(
            CreateLocationTypeCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Title, cancellationToken);

            return await existing.MatchAsync(
                lt => new LocationTypeAlreadyExistsException(lt.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<LocationTypeException, LocationType>> CreateEntity(
            CreateLocationTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newLocationType = LocationType.New(request.Title);
                var result = await repository.AddAsync(newLocationType, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledLocationTypeException(LocationTypeId.Empty(), ex);
            }
        }
    }
}