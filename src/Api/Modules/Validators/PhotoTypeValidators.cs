using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreatePhotoTypeDtoValidator : AbstractValidator<CreatePhotoTypeDto>
    {
        public CreatePhotoTypeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.")
                .MinimumLength(2).WithMessage("Title must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }

    public class UpdatePhotoTypeDtoValidator : AbstractValidator<UpdatePhotoTypeDto>
    {
        public UpdatePhotoTypeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.")
                .MinimumLength(2).WithMessage("Title must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
}