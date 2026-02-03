using FluentValidation;

namespace Application.Entities.LocationTypes.Commands
{
    public class DeleteLocationTypeCommandValidator : AbstractValidator<DeleteLocationTypeCommand>
    {
        public DeleteLocationTypeCommandValidator()
        {
            RuleFor(x => x.LocationTypeId)
                .NotEmpty().WithMessage("Location type ID must be provided.");
        }
    }
}