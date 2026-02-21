using FluentValidation;

namespace Application.Entities.DetectedTools.Commands
{
    public class CreateDetectedToolsCommandItemValidator
        : AbstractValidator<CreateDetectedToolsCommandItem>
    {
        public CreateDetectedToolsCommandItemValidator()
        {
            RuleFor(x => x.PhotoSessionId)
                .NotEmpty()
                .WithMessage("PhotoSessionId is required.");

            RuleFor(x => x.ToolTypeId)
                .NotEmpty()
                .WithMessage("ToolTypeId is required.");

            // BrandId / ModelId можуть бути null, але якщо задані — не Guid.Empty
            When(x => x.BrandId.HasValue, () =>
            {
                RuleFor(x => x.BrandId!.Value)
                    .NotEmpty()
                    .WithMessage("BrandId, if specified, must not be empty.");
            });

            When(x => x.ModelId.HasValue, () =>
            {
                RuleFor(x => x.ModelId!.Value)
                    .NotEmpty()
                    .WithMessage("ModelId, if specified, must not be empty.");
            });

            // SerialNumber — опціональне, але якщо є, можна трохи підчистити вимоги
            RuleFor(x => x.SerialNumber)
                .MaximumLength(200)
                .WithMessage("SerialNumber must not exceed 200 characters.");

            // Confidence — required float; зазвичай це 0..1
            RuleFor(x => x.Confidence)
                .GreaterThanOrEqualTo(0f)
                .WithMessage("Confidence must be greater than or equal to 0.")
                .LessThanOrEqualTo(1f)
                .WithMessage("Confidence must be less than or equal to 1.");

            // RedFlagged — bool, завжди є, ок, додаткових правил не потрібно
        }
    }
}