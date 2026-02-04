using FluentValidation;

namespace Application.Entities.DetectedTools.Commands
{
    public class DeleteDetectedToolCommandValidator : AbstractValidator<DeleteDetectedToolCommand>
    {
        public DeleteDetectedToolCommandValidator()
        {
            RuleFor(x => x.DetectedToolId)
                .NotEmpty().WithMessage("Detected tool ID must be provided.");
        }
    }
}