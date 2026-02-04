using FluentValidation;

namespace Application.Entities.Models.Commands
{
    public class DeleteModelCommandValidator : AbstractValidator<DeleteModelCommand>
    {
        public DeleteModelCommandValidator()
        {
            RuleFor(x => x.ModelId)
                .NotEmpty().WithMessage("Model ID must be provided.");
        }
    }
}