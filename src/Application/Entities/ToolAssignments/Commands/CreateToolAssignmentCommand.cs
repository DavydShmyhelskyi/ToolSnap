using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using Domain.Models.ToolLiabilities;
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
        public DateTime? DueAt { get; init; }
    }

    public class CreateToolAssignmentCommandHandler(
        IToolAssignmentsRepository repository,
        IToolLiabilityRepository liabilityRepository,
        IApplicationDbContext dbContext,
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

            var detectedTool = await detectedToolQueries.GetByIdAsync(takenDetectedToolId, cancellationToken);
            if (detectedTool.IsNone)
                return new DetectedToolNotFoundForToolAssignmentException(takenDetectedToolId);

            var toolOpt = await toolsQueries.GetByIdAsync(toolId, cancellationToken);
            if (toolOpt.IsNone)
                return new ToolNotFoundForToolAssignmentException(toolId);

            var user = await usersQueries.GetByIdAsync(userId, cancellationToken);
            if (user.IsNone)
                return new UserNotFoundForToolAssignmentException(userId);

            var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
            if (location.IsNone)
                return new LocationNotFoundForToolAssignmentException(locationId);

            var tool = toolOpt.Match(t => t, () => null!);

            return await CreateEntity(request, tool.Price, cancellationToken);
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> CreateEntity(
            CreateToolAssignmentCommand request,
            decimal toolPrice,
            CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);
            try
            {
                var now = DateTime.UtcNow;

                var newAssignment = ToolAssignment.New(
                    new DetectedToolId(request.TakenDetectedToolId),
                    new ToolId(request.ToolId),
                    new UserId(request.UserId),
                    new LocationId(request.LocationId),
                    now,
                    request.DueAt);

                var result = await repository.AddAsync(newAssignment, cancellationToken);

                var liability = ToolLiability.New(
                    new ToolId(request.ToolId),
                    result.Id,
                    new UserId(request.UserId),
                    toolPrice,
                    result.TakenAt);

                await liabilityRepository.AddAsync(liability, cancellationToken);

                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new UnhandledToolAssignmentException(ToolAssignmentId.Empty(), ex);
            }
        }
    }
}