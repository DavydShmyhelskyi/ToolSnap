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
}