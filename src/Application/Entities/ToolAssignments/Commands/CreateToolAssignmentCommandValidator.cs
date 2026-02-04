using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class CreateToolAssignmentCommandValidator : AbstractValidator<CreateToolAssignmentCommand>
    {
        public CreateToolAssignmentCommandValidator()
        {
            RuleFor(x => x.TakenDetectedToolId)
                .NotEmpty().WithMessage("Taken detected tool ID must be provided.");

            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("Tool ID must be provided.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID must be provided.");

            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("Location ID must be provided.");
        }
    }
}