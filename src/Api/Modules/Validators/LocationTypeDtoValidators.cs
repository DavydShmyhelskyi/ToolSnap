using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreateLocationTypeDtoValidator : AbstractValidator<CreateLocationTypeDto>
    {
        public CreateLocationTypeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.")
                .MinimumLength(2).WithMessage("Title must contain at least 2 characters.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
    public class UpdateLocationTypeDtoValidator : AbstractValidator<UpdateLocationTypeDto>
    {
        public UpdateLocationTypeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.")
                .MinimumLength(2).WithMessage("Title must contain at least 2 characters.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
}