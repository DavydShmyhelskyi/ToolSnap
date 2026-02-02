using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class CreateToolAssignmentCommandValidator : AbstractValidator<CreateToolAssignmentCommand>
    {
        public CreateToolAssignmentCommandValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("Tool ID is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("Location ID is required.");
        }
    }
}