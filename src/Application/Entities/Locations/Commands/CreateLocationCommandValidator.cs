using FluentValidation;

namespace Application.Entities.Locations.Commands
{
    public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Location name is required.")
                .MaximumLength(100).WithMessage("Location name must not exceed 100 characters.");
            
            RuleFor(x => x.LocationTypeId)
                .NotEmpty().WithMessage("Location type is required.");
            
            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Address));
            
            /*RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90 degrees.")
                .When(x => x.Latitude.HasValue);
            
            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180 degrees.")
                .When(x => x.Longitude.HasValue);*/
        }
    }
}
