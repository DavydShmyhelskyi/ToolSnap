using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolAssignments.Commands
{
    public record ReturnToolAssignmentLocationCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid ToolAssignmentId { get; init; }
        public required Guid LocationId { get; init; }
    }

    public class ReturnToolAssignmentLocationCommandHandler(
        IToolAssignmentQueries toolAssignmentQueries,
        ILocationsQueries locationsQueries,
        IToolAssignmentsRepository repository)
        : IRequestHandler<ReturnToolAssignmentLocationCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            ReturnToolAssignmentLocationCommand request,
            CancellationToken cancellationToken)
        {
            var toolAssignmentId = new ToolAssignmentId(request.ToolAssignmentId);
            var locationId = new LocationId(request.LocationId);

            // Перевірка існування ToolAssignment
            var toolAssignment = await toolAssignmentQueries.GetByIdAsync(toolAssignmentId, cancellationToken);
            if (toolAssignment.IsNone)
                return new ToolAssignmentNotFoundException(toolAssignmentId);

            // Перевірка існування Location
            var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
            if (location.IsNone)
                return new LocationNotFoundForToolAssignmentException(locationId);

            return await toolAssignment.MatchAsync(
                ta => SetReturnedLocation(ta, locationId, cancellationToken),
                () => new ToolAssignmentNotFoundException(toolAssignmentId));
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> SetReturnedLocation(
            ToolAssignment entity,
            LocationId returnedLocationId,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Return(returnedLocationId);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(entity.Id, ex);
            }
        }
    }
}