using Application.Common.Interfaces;
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
        IToolLiabilityQueries liabilityQueries,
        ILocationsQueries locationsQueries,
        IDetectedToolQueries detectedToolQueries,
        IToolAssignmentsRepository repository,
        IToolLiabilityRepository liabilityRepository,
        IApplicationDbContext dbContext)
        : IRequestHandler<ReturnToolAssignmentCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            ReturnToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            var toolAssignmentId = new ToolAssignmentId(request.ToolAssignmentId);
            var locationId = new LocationId(request.LocationId);
            var detectedToolId = new DetectedToolId(request.ReturnedDetectedToolId);

            var toolAssignment = await toolAssignmentQueries.GetByIdAsync(toolAssignmentId, cancellationToken);
            if (toolAssignment.IsNone)
                return new ToolAssignmentNotFoundException(toolAssignmentId);

            var location = await locationsQueries.GetByIdAsync(locationId, cancellationToken);
            if (location.IsNone)
                return new LocationNotFoundForToolAssignmentException(locationId);

            var detectedTool = await detectedToolQueries.GetByIdAsync(detectedToolId, cancellationToken);
            if (detectedTool.IsNone)
                return new DetectedToolNotFoundForToolAssignmentException(detectedToolId);

            return await toolAssignment.MatchAsync(
                ta => SetReturnedLocation(ta, toolAssignmentId, locationId, detectedToolId, cancellationToken),
                () => new ToolAssignmentNotFoundException(toolAssignmentId));
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> SetReturnedLocation(
            ToolAssignment entity,
            ToolAssignmentId toolAssignmentId,
            LocationId returnedLocationId,
            DetectedToolId returnedDetectedToolId,
            CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);
            try
            {
                entity.Return(returnedLocationId, returnedDetectedToolId);
                var result = await repository.UpdateAsync(entity, cancellationToken);

                var liabilityOpt = await liabilityQueries.GetOpenByToolAssignmentIdAsync(toolAssignmentId, cancellationToken);
                await liabilityOpt.IfSomeAsync(async liability =>
                {
                    liability.Close();
                    await liabilityRepository.UpdateAsync(liability, cancellationToken);
                });

                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new UnhandledToolAssignmentException(entity.Id, ex);
            }
        }
    }
}