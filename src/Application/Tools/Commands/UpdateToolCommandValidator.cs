using FluentValidation;

namespace Application.Tools.Commands
{
    public class UpdateToolCommandValidator : AbstractValidator<UpdateToolCommand>
    {
        public UpdateToolCommandValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("Tool ID must be provided.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty().WithMessage("Tool status ID must be provided.");
        }
    }
}