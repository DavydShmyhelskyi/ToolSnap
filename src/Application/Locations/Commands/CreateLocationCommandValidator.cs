using FluentValidation;

namespace Application.Locations.Commands
{
    public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Location name is required.")
                .MaximumLength(100).WithMessage("Location name must not exceed 100 characters.");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");
        }
    }
}
