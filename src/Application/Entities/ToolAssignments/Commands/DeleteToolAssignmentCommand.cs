using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.ToolAssignments;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolAssignments.Commands
{
    public record DeleteToolAssignmentCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid ToolAssignmentId { get; init; }
    }

    public class DeleteToolAssignmentCommandHandler(
        IToolAssignmentQueries queries,
        IToolAssignmentsRepository repository)
        : IRequestHandler<DeleteToolAssignmentCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            DeleteToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolAssignmentId(request.ToolAssignmentId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<ToolAssignmentException, ToolAssignment>>(
                ta => repository.DeleteAsync(ta, cancellationToken).Result,
                () => new ToolAssignmentNotFoundException(id));
        }
    }
}