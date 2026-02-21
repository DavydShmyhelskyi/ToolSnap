using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto>
    {
        public CreateBrandDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot be whitespace only.")
                .MinimumLength(2).WithMessage("Title must contain at least 2 characters.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
    public class UpdateBrandDtoValidator : AbstractValidator<UpdateBrandDto>
    {
        public UpdateBrandDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot be whitespace only.")
                .MinimumLength(2).WithMessage("Title must contain at least 2 characters.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
}