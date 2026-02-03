using FluentValidation;

namespace Application.Entities.ToolPhotos.Commands
{
    public class DeleteToolPhotoCommandValidator : AbstractValidator<DeleteToolPhotoCommand>
    {
        public DeleteToolPhotoCommandValidator()
        {
            RuleFor(x => x.ToolPhotoId)
                .NotEmpty().WithMessage("Tool photo ID must be provided.");
        }
    }
}