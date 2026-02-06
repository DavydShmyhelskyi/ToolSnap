using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolAssignments.Commands
{
    public record ReturnToolAssignmentCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid ToolAssignmentId { get; init; }
        public required Guid LocationId { get; init; }
        public required Guid ReturnedDetectedToolId { get; init; }
    }

    public class ReturnToolAssignmentCommandHandler(
        IToolAssignmentQueries toolAssignmentQueries,
        ILocationsQueries locationsQueries,
        IDetectedToolQueries detectedToolQueries,
        IToolAssignmentsRepository repository)
        : IRequestHandler<ReturnToolAssignmentCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            ReturnToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            var toolAssignmentId = new ToolAssignmentId(request.ToolAssignmentId);
            var locationId = new LocationId(request.LocationId);
            var detectedToolId = new DetectedToolId(request.ReturnedDetectedToolId);

            // Перевірка існування ToolAssignment
            var toolAssignment = await toolAssignmentQueries.GetByIdAsync(toolAssignmentId, cancellationToken);
            if (toolAssignment.IsNone)
                return new ToolAssignmentNotFoundException(toolAssignmentId);

            // Перевірка існування Location
            var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
            if (location.IsNone)
                return new LocationNotFoundForToolAssignmentException(locationId);

            // Перевірка існування DetectedTool
            var detectedTool = await detectedToolQueries.GetByIdAsync(detectedToolId, cancellationToken);
            if (detectedTool.IsNone)
                return new DetectedToolNotFoundForToolAssignmentException(detectedToolId);

            return await toolAssignment.MatchAsync(
                ta => SetReturnedLocation(ta, locationId, detectedToolId, cancellationToken),
                () => new ToolAssignmentNotFoundException(toolAssignmentId));
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> SetReturnedLocation(
            ToolAssignment entity,
            LocationId returnedLocationId,
            DetectedToolId returnedDetectedToolId,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Return(returnedLocationId, returnedDetectedToolId);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(entity.Id, ex);
            }
        }
    }
}