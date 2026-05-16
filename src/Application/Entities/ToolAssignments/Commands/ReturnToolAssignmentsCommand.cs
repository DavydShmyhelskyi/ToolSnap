using Application.Common.Interfaces;
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
        IToolLiabilityQueries liabilityQueries,
        ILocationsQueries locationsQueries,
        IDetectedToolQueries detectedToolQueries,
        IToolAssignmentsRepository repository,
        IToolLiabilityRepository liabilityRepository,
        IApplicationDbContext dbContext)
        : IRequestHandler<ReturnToolAssignmentsCommand, Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>>
    {
        public async Task<Either<ToolAssignmentException, IReadOnlyList<ToolAssignment>>> Handle(
            ReturnToolAssignmentsCommand request,
            CancellationToken cancellationToken)
        {
            var toUpdate = new List<ToolAssignment>(request.Items.Count);
            var assignmentIds = new List<ToolAssignmentId>(request.Items.Count);

            foreach (var item in request.Items)
            {
                var toolAssignmentId = new ToolAssignmentId(item.ToolAssignmentId);
                var locationId = new LocationId(item.LocationId);
                var detectedToolId = new DetectedToolId(item.ReturnedDetectedToolId);

                var toolAssignmentOpt = await toolAssignmentQueries.GetByIdAsync(toolAssignmentId, cancellationToken);
                if (toolAssignmentOpt.IsNone)
                    return new ToolAssignmentNotFoundException(toolAssignmentId);

                var toolAssignment = toolAssignmentOpt.Match(a => a, () => null)!;

                var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
                if (location.IsNone)
                    return new LocationNotFoundForToolAssignmentException(locationId);

                var detectedTool = await detectedToolQueries.GetByIdAsync(detectedToolId, cancellationToken);
                if (detectedTool.IsNone)
                    return new DetectedToolNotFoundForToolAssignmentException(detectedToolId);

                toolAssignment.Return(locationId, detectedToolId);
                toUpdate.Add(toolAssignment);
                assignmentIds.Add(toolAssignmentId);
            }

            using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await repository.UpdateRangeAsync(toUpdate, cancellationToken);

                foreach (var assignmentId in assignmentIds)
                {
                    var liabilityOpt = await liabilityQueries.GetOpenByToolAssignmentIdAsync(assignmentId, cancellationToken);
                    await liabilityOpt.IfSomeAsync(async liability =>
                    {
                        liability.Close();
                        await liabilityRepository.UpdateAsync(liability, cancellationToken);
                    });
                }

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