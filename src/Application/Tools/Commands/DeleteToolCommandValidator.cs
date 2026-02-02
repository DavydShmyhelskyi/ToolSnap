using FluentValidation;

namespace Application.Tools.Commands
{
    public class DeleteToolCommandValidator : AbstractValidator<DeleteToolCommand>
    {
        public DeleteToolCommandValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("Tool ID must be provided.");
        }
    }
}