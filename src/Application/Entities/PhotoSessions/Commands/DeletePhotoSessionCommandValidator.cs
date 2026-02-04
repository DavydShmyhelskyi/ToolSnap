using FluentValidation;

namespace Application.Entities.PhotoSessions.Commands
{
    public class DeletePhotoSessionCommandValidator : AbstractValidator<DeletePhotoSessionCommand>
    {
        public DeletePhotoSessionCommandValidator()
        {
            RuleFor(x => x.PhotoSessionId)
                .NotEmpty().WithMessage("Photo session ID must be provided.");
        }
    }
}