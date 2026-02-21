using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreateLocationDtoValidator : AbstractValidator<CreateLocationDto>
    {
        public CreateLocationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Name cannot contain only whitespace.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.LocationTypeId)
                .NotEmpty().WithMessage("LocationTypeId is required.");

            RuleFor(x => x.Address)
                .MaximumLength(300)
                .WithMessage("Address must not exceed 300 characters.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180.");
        }
    }
    public class UpdateLocationDtoValidator : AbstractValidator<UpdateLocationDto>
    {
        public UpdateLocationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Name cannot contain only whitespace.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.LocationTypeId)
                .NotEmpty().WithMessage("LocationTypeId is required.");

            RuleFor(x => x.Address)
                .MaximumLength(300)
                .WithMessage("Address must not exceed 300 characters.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180.");
        }
    }
}