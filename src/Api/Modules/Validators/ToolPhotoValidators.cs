using FluentValidation;
using Api.DTOs;
using Microsoft.AspNetCore.Http;

namespace Api.Modules.Validators
{
    public class CreateToolPhotoDtoValidator : AbstractValidator<CreateToolPhotoDto>
    {
        public CreateToolPhotoDtoValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty()
                .WithMessage("ToolId is required.");

            RuleFor(x => x.PhotoTypeId)
                .NotEmpty()
                .WithMessage("PhotoTypeId is required.");

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.")
                .Must(f => f.Length > 0)
                .WithMessage("Uploaded file cannot be empty.")
                .Must(BeValidImage)
                .WithMessage("Only image files are allowed (jpg, jpeg, png).")
                .Must(f => f.Length <= 10 * 1024 * 1024)
                .WithMessage("File size must not exceed 10 MB.");
        }

        private bool BeValidImage(IFormFile file)
        {
            if (file == null)
                return false;

            var allowedTypes = new[]
            {
                "image/jpeg",
                "image/jpg",
                "image/png"
            };

            return allowedTypes.Contains(file.ContentType.ToLower());
        }
    }
}