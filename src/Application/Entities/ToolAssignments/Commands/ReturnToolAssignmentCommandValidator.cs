using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class ReturnToolAssignmentCommandValidator : AbstractValidator<ReturnToolAssignmentLocationCommand>
    {
        public ReturnToolAssignmentCommandValidator()
        {

            RuleFor(x => x.ToolAssignmentId)
                .NotEmpty().WithMessage("Tool assignment ID must be provided.");

            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("Location ID must be provided.");
        }
    }
}