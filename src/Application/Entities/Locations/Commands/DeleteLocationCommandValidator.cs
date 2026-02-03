using FluentValidation;

namespace Application.Entities.Locations.Commands
{
    public class DeleteLocationCommandValidator : AbstractValidator<DeleteLocationCommand>
    {
        public DeleteLocationCommandValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location ID must be provided.");
        }
    }
}
