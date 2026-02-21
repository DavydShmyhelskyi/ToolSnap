using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;

namespace Application.Entities.ToolAssignments.Commands
{
    public record CreateToolAssignmentsCommandItem
    {
        public required Guid TakenDetectedToolId { get; init; }
        public required Guid ToolId { get; init; }
        public required Guid UserId { get; init; }
        public required Guid LocationId { get; init; }
    }

    public record CreateToolAssignmentsCommand
        : IRequest<Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>>
    {
        public required IReadOnlyList<CreateToolAssignmentsCommandItem> Items { get; init; }
    }

    public class CreateToolAssignmentsCommandHandler(
        IToolAssignmentsRepository repository,
        IDetectedToolQueries detectedToolQueries,
        IToolsQueries toolsQueries,
        IUsersQueries usersQueries,
        ILocationsQueries locationsQueries)
        : IRequestHandler<CreateToolAssignmentsCommand, Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>>
    {
        public async Task<Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>> Handle(
            CreateToolAssignmentsCommand request,
            CancellationToken cancellationToken)
        {
            /*if (request.Items is null || request.Items.Count == 0)
            {
                return new UnhandledToolAssignmentException(
                    ToolAssignmentId.Empty(),
                    new ArgumentException("Tool assignments collection is empty.", nameof(request.Items)));
            }*/

            var created = new List<ToolAssignment>(request.Items.Count);

            try
            {
                foreach (var item in request.Items)
                {
                    var takenDetectedToolId = new DetectedToolId(item.TakenDetectedToolId);
                    var toolId = new ToolId(item.ToolId);
                    var userId = new UserId(item.UserId);
                    var locationId = new LocationId(item.LocationId);

                    // DetectedTool
                    var detectedTool = await detectedToolQueries.GetByIdAsync(takenDetectedToolId, cancellationToken);
                    if (detectedTool.IsNone)
                        return new DetectedToolNotFoundForToolAssignmentException(takenDetectedToolId);

                    // Tool
                    var tool = await toolsQueries.GetByIdAsync(toolId, cancellationToken);
                    if (tool.IsNone)
                        return new ToolNotFoundForToolAssignmentException(toolId);

                    // User
                    var user = await usersQueries.GetByIdAsync(userId, cancellationToken);
                    if (user.IsNone)
                        return new UserNotFoundForToolAssignmentException(userId);

                    // Location
                    var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
                    if (location.IsNone)
                        return new LocationNotFoundForToolAssignmentException(locationId);

                    var assignment = ToolAssignment.New(
                        takenDetectedToolId,
                        toolId,
                        userId,
                        locationId,
                        DateTime.UtcNow);

                    created.Add(assignment);
                }

                var result = await repository.AddRangeAsync(created, cancellationToken);
                return Right<ToolAssignmentException, IReadOnlyList<ToolAssignment>>(result);
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(ToolAssignmentId.Empty(), ex);
            }
        }
    }
}