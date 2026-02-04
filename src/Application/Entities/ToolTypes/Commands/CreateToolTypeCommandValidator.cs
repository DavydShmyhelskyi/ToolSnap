using FluentValidation;

namespace Application.Entities.ToolTypes.Commands
{
    public class CreateToolTypeCommandValidator : AbstractValidator<CreateToolTypeCommand>
    {
        public CreateToolTypeCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tool type title is required.")
                .MaximumLength(100).WithMessage("Tool type title must not exceed 100 characters.");
        }
    }
}