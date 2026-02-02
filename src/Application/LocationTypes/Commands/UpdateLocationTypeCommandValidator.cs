using FluentValidation;

namespace Application.LocationTypes.Commands
{
    public class UpdateLocationTypeCommandValidator : AbstractValidator<UpdateLocationTypeCommand>
    {
        public UpdateLocationTypeCommandValidator()
        {
            RuleFor(x => x.LocationTypeId)
                .NotEmpty().WithMessage("Location type ID must be provided.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Location type title is required.")
                .MaximumLength(50).WithMessage("Location type title must not exceed 50 characters.");
        }
    }
}