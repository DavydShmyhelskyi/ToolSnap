using FluentValidation;

namespace Application.LocationTypes.Commands
{
    public class CreateLocationTypeCommandValidator : AbstractValidator<CreateLocationTypeCommand>
    {
        public CreateLocationTypeCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Location type title is required.")
                .MaximumLength(50).WithMessage("Location type title must not exceed 50 characters.");
        }
    }
}