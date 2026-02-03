using FluentValidation;

namespace Application.Entities.ToolStatuses.Commands
{
    public class CreateToolStatusCommandValidator : AbstractValidator<CreateToolStatusCommand>
    {
        public CreateToolStatusCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tool status title is required.")
                .MaximumLength(50).WithMessage("Tool status title must not exceed 50 characters.");
        }
    }
}