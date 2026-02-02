using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.ToolAssignments.Exceptions;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using LanguageExt;
using MediatR;

namespace Application.ToolAssignments.Commands
{
    public record UpdateToolAssignmentCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid ToolAssignmentId { get; init; }
        public required Guid LocationId { get; init; }
    }

    public class UpdateToolAssignmentCommandHandler(
        IToolAssignmentQueries queries,
        IToolAssigmentsRepository repository)
        : IRequestHandler<UpdateToolAssignmentCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            UpdateToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolAssignmentId(request.ToolAssignmentId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                ta => UpdateEntity(ta, request, cancellationToken),
                () => new ToolAssignmentNotFoundException(id));
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> UpdateEntity(
            ToolAssignment entity,
            UpdateToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var locationId = new LocationId(request.LocationId);
                entity.UpdateLocation(locationId);
                return await repository.Approve(cancellationToken);
            }
            catch (InvalidOperationException ex)
            {
                return new UnhandledToolAssignmentException(entity.Id, ex);
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(entity.Id, ex);
            }
        }
    }
}