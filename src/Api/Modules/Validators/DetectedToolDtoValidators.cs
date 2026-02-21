using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreateDetectedToolDtoValidator : AbstractValidator<CreateDetectedToolDto>
    {
        public CreateDetectedToolDtoValidator()
        {
            RuleFor(x => x.PhotoSessionId)
                .NotEmpty()
                .WithMessage("PhotoSessionId is required.");

            RuleFor(x => x.ToolTypeId)
                .NotEmpty()
                .WithMessage("ToolTypeId is required.");

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

            RuleFor(x => x.Confidence)
                .InclusiveBetween(0f, 1f)
                .WithMessage("Confidence must be between 0 and 1.");
        }
    }
    public class CreateDetectedToolBatchItemDtoValidator
        : AbstractValidator<CreateDetectedToolBatchItemDto>
    {
        public CreateDetectedToolBatchItemDtoValidator()
        {
            RuleFor(x => x.PhotoSessionId)
                .NotEmpty()
                .WithMessage("PhotoSessionId is required.");

            RuleFor(x => x.ToolTypeId)
                .NotEmpty()
                .WithMessage("ToolTypeId is required.");

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

            RuleFor(x => x.Confidence)
                .InclusiveBetween(0f, 1f)
                .WithMessage("Confidence must be between 0 and 1.");
        }
    }
    public class CreateDetectedToolsBatchDtoValidator
        : AbstractValidator<CreateDetectedToolsBatchDto>
    {
        public CreateDetectedToolsBatchDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Items list cannot be null.")
                .NotEmpty()
                .WithMessage("Items list cannot be empty.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateDetectedToolBatchItemDtoValidator());
        }
    }
}