using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.ToolAssignments;
using FluentValidation;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolAssignments.Commands
{
    public record SetToolAssignmentDueCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid ToolAssignmentId { get; init; }
        public DateTime? DueAt { get; init; }
    }

    public class SetToolAssignmentDueCommandHandler(
        IToolAssignmentQueries queries,
        IToolAssignmentsRepository repository)
        : IRequestHandler<SetToolAssignmentDueCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            SetToolAssignmentDueCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolAssignmentId(request.ToolAssignmentId);
            var assignmentOpt = await queries.GetByIdAsync(id, cancellationToken);

            if (assignmentOpt.IsNone)
                return new ToolAssignmentNotFoundException(id);

            return await assignmentOpt.MatchAsync(
                async assignment =>
                {
                    if (assignment.ReturnedAt != null)
                        return (Either<ToolAssignmentException, ToolAssignment>)
                            new ToolAssignmentAlreadyReturnedException(id);

                    if (request.DueAt.HasValue && request.DueAt.Value <= DateTime.UtcNow)
                        return new ToolAssignmentDueDateInPastException(id);

                    if (request.DueAt.HasValue)
                        assignment.SetDue(request.DueAt.Value);
                    else
                        assignment.ClearDue();

                    return await repository.UpdateAsync(assignment, cancellationToken);
                },
                () => new ToolAssignmentNotFoundException(id));
        }
    }

    public class SetToolAssignmentDueCommandValidator : AbstractValidator<SetToolAssignmentDueCommand>
    {
        public SetToolAssignmentDueCommandValidator()
        {
            RuleFor(x => x.ToolAssignmentId).NotEmpty();
        }
    }
}
