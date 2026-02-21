using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreateActionTypeDtoValidator : AbstractValidator<CreateActionTypeDto>
    {
        public CreateActionTypeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
                .MinimumLength(2).WithMessage("Title must contain at least 2 characters.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.");
        }
    }
    public class UpdateActionTypeDtoValidator : AbstractValidator<UpdateActionTypeDto>
    {
        public UpdateActionTypeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
                .MinimumLength(2).WithMessage("Title must contain at least 2 characters.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.");
        }
    }
}