using FluentValidation;

namespace Application.Entities.ToolStatuses.Commands
{
    public class DeleteToolStatusCommandValidator : AbstractValidator<DeleteToolStatusCommand>
    {
        public DeleteToolStatusCommandValidator()
        {
            RuleFor(x => x.ToolStatusId)
                .NotEmpty().WithMessage("Tool status ID must be provided.");
        }
    }
}