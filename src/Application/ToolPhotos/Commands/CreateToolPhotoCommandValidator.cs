using FluentValidation;

namespace Application.ToolPhotos.Commands
{
    public class CreateToolPhotoCommandValidator : AbstractValidator<CreateToolPhotoCommand>
    {
        public CreateToolPhotoCommandValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("Tool ID is required.");

            RuleFor(x => x.PhotoTypeId)
                .NotEmpty().WithMessage("Photo type ID is required.");

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