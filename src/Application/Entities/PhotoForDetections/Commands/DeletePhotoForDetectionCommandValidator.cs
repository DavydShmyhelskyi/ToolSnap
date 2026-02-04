using FluentValidation;

namespace Application.Entities.PhotoForDetections.Commands
{
    public class DeletePhotoForDetectionCommandValidator : AbstractValidator<DeletePhotoForDetectionCommand>
    {
        public DeletePhotoForDetectionCommandValidator()
        {
            RuleFor(x => x.PhotoForDetectionId)
                .NotEmpty().WithMessage("Photo for detection ID must be provided.");
        }
    }
}