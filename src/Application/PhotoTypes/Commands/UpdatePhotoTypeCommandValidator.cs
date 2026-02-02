using FluentValidation;

namespace Application.PhotoTypes.Commands
{
    public class UpdatePhotoTypeCommandValidator : AbstractValidator<UpdatePhotoTypeCommand>
    {
        public UpdatePhotoTypeCommandValidator()
        {
            RuleFor(x => x.PhotoTypeId)
                .NotEmpty().WithMessage("Photo type ID must be provided.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Photo type title is required.")
                .MaximumLength(50).WithMessage("Photo type title must not exceed 50 characters.");
        }
    }
}