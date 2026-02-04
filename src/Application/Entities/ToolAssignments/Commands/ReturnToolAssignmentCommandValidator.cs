using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class ReturnToolAssignmentCommandValidator : AbstractValidator<ReturnToolAssignmentCommand>
    {
        public ReturnToolAssignmentCommandValidator()
        {
            RuleFor(x => x.ToolAssignmentId)
                .NotEmpty().WithMessage("Tool assignment ID must be provided.");
        }
    }
}