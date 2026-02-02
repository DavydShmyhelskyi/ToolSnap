/*using FluentValidation;

namespace Application.ToolAssignments.Commands
{
    public class UpdateToolAssignmentCommandValidator : AbstractValidator<UpdateToolAssignmentCommand>
    {
        public UpdateToolAssignmentCommandValidator()
        {
            RuleFor(x => x.ToolAssignmentId)
                .NotEmpty().WithMessage("Tool assignment ID must be provided.");

            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("Location ID must be provided.");
        }
    }
}*/