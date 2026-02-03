using FluentValidation;

namespace Application.Entities.Tools.Commands
{
    public class CreateToolCommandValidator : AbstractValidator<CreateToolCommand>
    {
        public CreateToolCommandValidator()
        {
            RuleFor(x => x.ToolTypeId)
                .NotEmpty().WithMessage("Tool name is required.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty().WithMessage("Tool status is required.");

            RuleFor(x => x.SerialNumber)
                .MaximumLength(50).WithMessage("Serial number must not exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));
        }
    }
}