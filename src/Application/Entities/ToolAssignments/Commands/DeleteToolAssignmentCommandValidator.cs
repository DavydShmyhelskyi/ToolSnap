using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class DeleteToolAssignmentCommandValidator : AbstractValidator<DeleteToolAssignmentCommand>
    {
        public DeleteToolAssignmentCommandValidator()
        {
            RuleFor(x => x.ToolAssignmentId)
                .NotEmpty().WithMessage("Tool assignment ID must be provided.");
        }
    }
}