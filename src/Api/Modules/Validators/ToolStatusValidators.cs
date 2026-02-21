using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    public class CreateToolStatusDtoValidator : AbstractValidator<CreateToolStatusDto>
    {
        public CreateToolStatusDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.")
                .MinimumLength(2).WithMessage("Title must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }

    public class UpdateToolStatusDtoValidator : AbstractValidator<UpdateToolStatusDto>
    {
        public UpdateToolStatusDtoValidator()
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