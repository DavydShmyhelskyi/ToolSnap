using FluentValidation;

namespace Application.Entities.Locations.Commands
{
    public class ActivateLocationCommandValidator : AbstractValidator<ActivateLocationCommand>
    {
        public ActivateLocationCommandValidator()
        {
            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("Location ID must be provided.");
        }
    }
}