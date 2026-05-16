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
using static LanguageExt.Prelude;

namespace Application.Entities.ToolAssignments.Commands
{
    public record CreateToolAssignmentsCommandItem
    {
        public required Guid TakenDetectedToolId { get; init; }
        public required Guid ToolId { get; init; }
        public required Guid UserId { get; init; }
        public required Guid LocationId { get; init; }
        public DateTime? DueAt { get; init; }
    }

    public record CreateToolAssignmentsCommand
        : IRequest<Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>>
    {
        public required IReadOnlyList<CreateToolAssignmentsCommandItem> Items { get; init; }
    }

    public class CreateToolAssignmentsCommandHandler(
        IToolAssignmentsRepository repository,
        IToolLiabilityRepository liabilityRepository,
        IApplicationDbContext dbContext,
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
            var created = new List<ToolAssignment>(request.Items.Count);
            var toolPrices = new Dictionary<Guid, decimal>(request.Items.Count);

            foreach (var item in request.Items)
            {
                var takenDetectedToolId = new DetectedToolId(item.TakenDetectedToolId);
                var toolId = new ToolId(item.ToolId);
                var userId = new UserId(item.UserId);
                var locationId = new LocationId(item.LocationId);

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
                toolPrices[item.ToolId] = tool.Price;

                created.Add(ToolAssignment.New(takenDetectedToolId, toolId, userId, locationId, DateTime.UtcNow, item.DueAt));
            }

            using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await repository.AddRangeAsync(created, cancellationToken);

                var liabilities = result.Select(a => ToolLiability.New(
                    a.ToolId,
                    a.Id,
                    a.UserId,
                    toolPrices[a.ToolId.Value],
                    a.TakenAt)).ToList();

                await liabilityRepository.AddRangeAsync(liabilities, cancellationToken);

                transaction.Commit();
                return Right<ToolAssignmentException, IReadOnlyList<ToolAssignment>>(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new UnhandledToolAssignmentException(ToolAssignmentId.Empty(), ex);
            }
        }
    }
}