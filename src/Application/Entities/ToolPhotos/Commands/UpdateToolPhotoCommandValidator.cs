using FluentValidation;

namespace Application.Entities.ToolPhotos.Commands
{
    public class UpdateToolPhotoCommandValidator : AbstractValidator<UpdateToolPhotoCommand>
    {
        public UpdateToolPhotoCommandValidator()
        {
            RuleFor(x => x.ToolPhotoId)
                .NotEmpty().WithMessage("Tool photo ID must be provided.");

            RuleFor(x => x.PhotoTypeId)
                .NotEmpty().WithMessage("Photo type ID must be provided.");
        }
    }
}