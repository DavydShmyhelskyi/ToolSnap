using FluentValidation;

namespace Application.Entities.ToolPhotos.Commands
{
    public class GetToolPhotoFilesByToolCommandValidator
        : AbstractValidator<GetToolPhotoFilesByToolCommand>
    {
        public GetToolPhotoFilesByToolCommandValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty()
                .WithMessage("ToolId is required.");
        }
    }
}