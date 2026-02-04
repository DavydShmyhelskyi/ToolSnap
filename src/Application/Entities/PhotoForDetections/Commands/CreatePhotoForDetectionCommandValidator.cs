using FluentValidation;

namespace Application.Entities.PhotoForDetections.Commands
{
    public class CreatePhotoForDetectionCommandValidator : AbstractValidator<CreatePhotoForDetectionCommand>
    {
        public CreatePhotoForDetectionCommandValidator()
        {
            RuleFor(x => x.PhotoSessionId)
                .NotEmpty().WithMessage("Photo session ID is required.");

            RuleFor(x => x.OriginalName)
                .NotEmpty().WithMessage("Original file name is required.")
                .MaximumLength(255).WithMessage("File name must not exceed 255 characters.")
                .Must(BeValidFileName).WithMessage("File name contains invalid characters.");
        }

        private bool BeValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var invalidChars = Path.GetInvalidFileNameChars();
            return !fileName.Any(c => invalidChars.Contains(c));
        }
    }
}