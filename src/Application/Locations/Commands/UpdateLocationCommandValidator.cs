using FluentValidation;

namespace Application.Locations.Commands
{
    public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Location name is required.")
                .MaximumLength(100).WithMessage("Location name must not exceed 100 characters.");
        }

    }
}
