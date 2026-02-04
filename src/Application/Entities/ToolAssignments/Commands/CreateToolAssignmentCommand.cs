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

namespace Application.Entities.ToolAssignments.Commands
{
    public record CreateToolAssignmentCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid TakenDetectedToolId { get; init; }
        public required Guid ToolId { get; init; }
        public required Guid UserId { get; init; }
        public required Guid LocationId { get; init; }
    }

    public class CreateToolAssignmentCommandHandler(
        IToolAssignmentsRepository repository,
        IDetectedToolQueries detectedToolQueries,
        IToolsQueries toolsQueries,
        IUsersQueries usersQueries,
        ILocationsQueries locationsQueries)
        : IRequestHandler<CreateToolAssignmentCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            CreateToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            var takenDetectedToolId = new DetectedToolId(request.TakenDetectedToolId);
            var toolId = new ToolId(request.ToolId);
            var userId = new UserId(request.UserId);
            var locationId = new LocationId(request.LocationId);

            // Перевірка існування DetectedTool
            var detectedTool = await detectedToolQueries.GetByIdAsync(takenDetectedToolId, cancellationToken);
            if (detectedTool.IsNone)
                return new DetectedToolNotFoundForToolAssignmentException(takenDetectedToolId);

            // Перевірка існування Tool
            var tool = await toolsQueries.GetByIdAsync(toolId, cancellationToken);
            if (tool.IsNone)
                return new ToolNotFoundForToolAssignmentException(toolId);

            // Перевірка існування User
            var user = await usersQueries.GetByIdAsync(userId, cancellationToken);
            if (user.IsNone)
                return new UserNotFoundForToolAssignmentException(userId);

            // Перевірка існування Location
            var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
            if (location.IsNone)
                return new LocationNotFoundForToolAssignmentException(locationId);

            return await CreateEntity(request, cancellationToken);
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> CreateEntity(
            CreateToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var takenDetectedToolId = new DetectedToolId(request.TakenDetectedToolId);
                var toolId = new ToolId(request.ToolId);
                var userId = new UserId(request.UserId);
                var locationId = new LocationId(request.LocationId);

                var newAssignment = ToolAssignment.New(
                    takenDetectedToolId,
                    toolId,
                    userId,
                    locationId,
                    DateTimeOffset.UtcNow);

                var result = await repository.AddAsync(newAssignment, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(ToolAssignmentId.Empty(), ex);
            }
        }
    }
}