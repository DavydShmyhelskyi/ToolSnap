using FluentValidation;

namespace Application.Entities.Tools.Commands
{
    public class ChangeToolStatusCommandValidator : AbstractValidator<ChangeToolStatusCommand>
    {
        public ChangeToolStatusCommandValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("Tool ID must be provided.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty().WithMessage("Tool status ID must be provided.");
        }
    }
}