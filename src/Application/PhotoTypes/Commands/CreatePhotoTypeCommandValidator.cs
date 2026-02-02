using FluentValidation;

namespace Application.PhotoTypes.Commands
{
    public class CreatePhotoTypeCommandValidator : AbstractValidator<CreatePhotoTypeCommand>
    {
        public CreatePhotoTypeCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Photo type title is required.")
                .MaximumLength(50).WithMessage("Photo type title must not exceed 50 characters.");
        }
    }
}