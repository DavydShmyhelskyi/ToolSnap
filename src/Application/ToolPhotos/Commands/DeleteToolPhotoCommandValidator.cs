using FluentValidation;

namespace Application.ToolPhotos.Commands
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