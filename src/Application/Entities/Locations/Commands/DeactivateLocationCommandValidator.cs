using FluentValidation;

namespace Application.Entities.Locations.Commands
{
    public class DeactivateLocationCommandValidator : AbstractValidator<DeactivateLocationCommand>
    {
        public DeactivateLocationCommandValidator()
        {
            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("Location ID must be provided.");
        }
    }
}