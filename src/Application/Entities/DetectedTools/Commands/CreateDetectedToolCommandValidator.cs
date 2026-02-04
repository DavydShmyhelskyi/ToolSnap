using FluentValidation;

namespace Application.Entities.DetectedTools.Commands
{
    public class CreateDetectedToolCommandValidator : AbstractValidator<CreateDetectedToolCommand>
    {
        public CreateDetectedToolCommandValidator()
        {
            RuleFor(x => x.PhotoSessionId)
                .NotEmpty().WithMessage("Photo session ID is required.");

            RuleFor(x => x.ToolTypeId)
                .NotEmpty().WithMessage("Tool type ID is required.");

            RuleFor(x => x.Confidence)
                .InclusiveBetween(0f, 1f).WithMessage("Confidence must be between 0 and 1.");

            RuleFor(x => x.SerialNumber)
                .MaximumLength(100).WithMessage("Serial number must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));
        }
    }
}