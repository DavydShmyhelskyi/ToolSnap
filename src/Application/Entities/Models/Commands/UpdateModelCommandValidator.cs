using FluentValidation;

namespace Application.Entities.Models.Commands
{
    public class UpdateModelCommandValidator : AbstractValidator<UpdateModelCommand>
    {
        public UpdateModelCommandValidator()
        {
            RuleFor(x => x.ModelId)
                .NotEmpty().WithMessage("Model ID must be provided.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Model title is required.")
                .MaximumLength(100).WithMessage("Model title must not exceed 100 characters.");
        }
    }
}