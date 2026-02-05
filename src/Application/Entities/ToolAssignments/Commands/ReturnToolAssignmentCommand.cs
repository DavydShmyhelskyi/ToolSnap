using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.ToolAssignments;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolAssignments.Commands
{
    public record ReturnToolAssignmentCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid ToolAssignmentId { get; init; }
    }

    public class ReturnToolAssignmentCommandHandler(
        IToolAssignmentQueries queries,
        IToolAssignmentsRepository repository)
        : IRequestHandler<ReturnToolAssignmentCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            ReturnToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolAssignmentId(request.ToolAssignmentId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                ta => ReturnToolAssignment(ta, cancellationToken),
                () => new ToolAssignmentNotFoundException(id));
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> ReturnToolAssignment(
            ToolAssignment entity,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Return(); // location Id
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            /*
             if(entity.IsReturned)
                 throw new InvalidOperationException("Tool assignment has already been returned.");
             
             */
            catch (InvalidOperationException ex)
            {
                return new ToolAssignmentAlreadyReturnedException(entity.Id); // ?
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(entity.Id, ex);
            }
        }
    }
}