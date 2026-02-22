using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreateToolDtoValidator : AbstractValidator<CreateToolDto>
    {
        public CreateToolDtoValidator()
        {
            RuleFor(x => x.ToolTypeId)
                .NotEmpty()
                .WithMessage("ToolTypeId is required.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty()
                .WithMessage("ToolStatusId is required.");

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

            RuleFor(x => x.SerialNumber)
                .MaximumLength(200)
                .WithMessage("SerialNumber must not exceed 200 characters.");
        }
    }

    public class UpdateToolDtoValidator : AbstractValidator<UpdateToolDto>
    {
        public UpdateToolDtoValidator()
        {
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

            RuleFor(x => x.SerialNumber)
                .MaximumLength(200)
                .WithMessage("SerialNumber must not exceed 200 characters.");
        }
    }

    public class ChangeToolStatusDtoValidator : AbstractValidator<ChangeToolStatusDto>
    {
        public ChangeToolStatusDtoValidator()
        {
            RuleFor(x => x.ToolStatusId)
                .NotEmpty()
                .WithMessage("ToolStatusId is required.");
        }
    }

    public class CreateToolWithAssignmentDtoValidator : AbstractValidator<CreateToolWithAssignmentDto>
    {
        public CreateToolWithAssignmentDtoValidator()
        {
            // USER
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            // ACTION TYPE
            RuleFor(x => x.ActionTypeId)
                .NotEmpty()
                .WithMessage("ActionTypeId is required.");

            // PHOTO TYPE (для ToolPhoto)
            RuleFor(x => x.PhotoTypeId)
                .NotEmpty()
                .WithMessage("PhotoTypeId is required.");

            // LOCATION
            RuleFor(x => x.LocationId)
                .NotEmpty()
                .WithMessage("LocationId is required.");

            // TOOL TYPE & STATUS
            RuleFor(x => x.ToolTypeId)
                .NotEmpty()
                .WithMessage("ToolTypeId is required.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty()
                .WithMessage("ToolStatusId is required.");

            // OPTIONAL BRAND
            When(x => x.BrandId.HasValue, () =>
            {
                RuleFor(x => x.BrandId!.Value)
                    .NotEmpty()
                    .WithMessage("BrandId, if specified, must not be empty.");
            });

            // OPTIONAL MODEL
            When(x => x.ModelId.HasValue, () =>
            {
                RuleFor(x => x.ModelId!.Value)
                    .NotEmpty()
                    .WithMessage("ModelId, if specified, must not be empty.");
            });

            // SERIAL NUMBER
            RuleFor(x => x.SerialNumber)
                .MaximumLength(200)
                .WithMessage("SerialNumber must not exceed 200 characters.");

            // FILE VALIDATION
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.")
                .Must(f => f.Length > 0)
                .WithMessage("Uploaded file cannot be empty.")
                .Must(BeValidImage)
                .WithMessage("Only image files are allowed (jpg, jpeg, png).")
                .Must(f => f.Length <= 10 * 1024 * 1024) // 10MB
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