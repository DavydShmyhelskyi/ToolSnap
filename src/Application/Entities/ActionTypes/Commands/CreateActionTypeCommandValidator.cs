using FluentValidation;

namespace Application.Entities.ActionTypes.Commands
{
    public class CreateActionTypeCommandValidator : AbstractValidator<CreateActionTypeCommand>
    {
        public CreateActionTypeCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Action type title is required.")
                .MaximumLength(50).WithMessage("Action type title must not exceed 50 characters.");
        }
    }
}