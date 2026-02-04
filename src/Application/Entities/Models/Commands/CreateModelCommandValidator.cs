using FluentValidation;

namespace Application.Entities.Models.Commands
{
    public class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
    {
        public CreateModelCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Model title is required.")
                .MaximumLength(100).WithMessage("Model title must not exceed 100 characters.");
        }
    }
}