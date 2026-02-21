using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;

namespace Application.Entities.ToolAssignments.Commands
{
    public record ReturnToolAssignmentsCommandItem
    {
        public required Guid ToolAssignmentId { get; init; }
        public required Guid LocationId { get; init; }
        public required Guid ReturnedDetectedToolId { get; init; }
    }

    public record ReturnToolAssignmentsCommand
        : IRequest<Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>>
    {
        public required IReadOnlyList<ReturnToolAssignmentsCommandItem> Items { get; init; }
    }

    public class ReturnToolAssignmentsCommandHandler(
        IToolAssignmentQueries toolAssignmentQueries,
        ILocationsQueries locationsQueries,
        IDetectedToolQueries detectedToolQueries,
        IToolAssignmentsRepository repository)
        : IRequestHandler<ReturnToolAssignmentsCommand, Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>>
    {
        public async Task<Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>> Handle(
            ReturnToolAssignmentsCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Items is null || request.Items.Count == 0)
            {
                return new UnhandledToolAssignmentException(
                    ToolAssignmentId.Empty(),
                    new ArgumentException("Tool assignments collection is empty.", nameof(request.Items)));
            }

            var toUpdate = new List<ToolAssignment>(request.Items.Count);

            try
            {
                foreach (var item in request.Items)
                {
                    var toolAssignmentId = new ToolAssignmentId(item.ToolAssignmentId);
                    var locationId = new LocationId(item.LocationId);
                    var detectedToolId = new DetectedToolId(item.ReturnedDetectedToolId);

                    // ToolAssignment
                    var toolAssignmentOpt = await toolAssignmentQueries.GetByIdAsync(toolAssignmentId, cancellationToken);
                    if (toolAssignmentOpt.IsNone)
                        return new ToolAssignmentNotFoundException(toolAssignmentId);

                    var toolAssignment = toolAssignmentOpt.Match(a => a, () => null)!;

                    // Location
                    var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
                    if (location.IsNone)
                        return new LocationNotFoundForToolAssignmentException(locationId);

                    // DetectedTool
                    var detectedTool = await detectedToolQueries.GetByIdAsync(detectedToolId, cancellationToken);
                    if (detectedTool.IsNone)
                        return new DetectedToolNotFoundForToolAssignmentException(detectedToolId);

                    toolAssignment.Return(locationId, detectedToolId);
                    toUpdate.Add(toolAssignment);
                }

                var result = await repository.UpdateRangeAsync(toUpdate, cancellationToken);
                return Right<ToolAssignmentException, IReadOnlyList<ToolAssignment>>(result);
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(ToolAssignmentId.Empty(), ex);
            }
        }
    }
}