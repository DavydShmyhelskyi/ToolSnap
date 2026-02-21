using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    // CreateModelDto Validator
    public class CreateModelDtoValidator : AbstractValidator<CreateModelDto>
    {
        public CreateModelDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage("Title cannot consist only of whitespace.")
                .MinimumLength(2).WithMessage("Title must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }

    // UpdateModelDto Validator
    public class UpdateModelDtoValidator : AbstractValidator<UpdateModelDto>
    {
        public UpdateModelDtoValidator()
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