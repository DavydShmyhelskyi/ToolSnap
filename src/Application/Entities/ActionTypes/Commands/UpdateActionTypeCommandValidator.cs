using FluentValidation;

namespace Application.Entities.ActionTypes.Commands
{
    public class UpdateActionTypeCommandValidator : AbstractValidator<UpdateActionTypeCommand>
    {
        public UpdateActionTypeCommandValidator()
        {
            RuleFor(x => x.ActionTypeId)
                .NotEmpty().WithMessage("Action type ID must be provided.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Action type title is required.")
                .MaximumLength(50).WithMessage("Action type title must not exceed 50 characters.");
        }
    }
}