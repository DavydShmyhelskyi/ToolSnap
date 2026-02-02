using FluentValidation;

namespace Application.PhotoTypes.Commands
{
    public class DeletePhotoTypeCommandValidator : AbstractValidator<DeletePhotoTypeCommand>
    {
        public DeletePhotoTypeCommandValidator()
        {
            RuleFor(x => x.PhotoTypeId)
                .NotEmpty().WithMessage("Photo type ID must be provided.");
        }
    }
}