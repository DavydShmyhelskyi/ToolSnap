using FluentValidation;

namespace Application.Entities.ToolTypes.Commands
{
    public class UpdateToolTypeCommandValidator : AbstractValidator<UpdateToolTypeCommand>
    {
        public UpdateToolTypeCommandValidator()
        {
            RuleFor(x => x.ToolTypeId)
                .NotEmpty().WithMessage("Tool type ID must be provided.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tool type title is required.")
                .MaximumLength(100).WithMessage("Tool type title must not exceed 100 characters.");
        }
    }
}