using FluentValidation;

namespace Application.Entities.ActionTypes.Commands
{
    public class DeleteActionTypeCommandValidator : AbstractValidator<DeleteActionTypeCommand>
    {
        public DeleteActionTypeCommandValidator()
        {
            RuleFor(x => x.ActionTypeId)
                .NotEmpty().WithMessage("Action type ID must be provided.");
        }
    }
}