using FluentValidation;

namespace Application.ToolStatuses.Commands
{
    public class UpdateToolStatusCommandValidator : AbstractValidator<UpdateToolStatusCommand>
    {
        public UpdateToolStatusCommandValidator()
        {
            RuleFor(x => x.ToolStatusId)
                .NotEmpty().WithMessage("Tool status ID must be provided.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tool status title is required.")
                .MaximumLength(50).WithMessage("Tool status title must not exceed 50 characters.");
        }
    }
}