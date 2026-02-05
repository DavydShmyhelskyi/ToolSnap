using FluentValidation;

namespace Application.Entities.Tools.Commands
{
    public class CreateToolCommandValidator : AbstractValidator<CreateToolCommand>
    {
        public CreateToolCommandValidator()
        {
            RuleFor(x => x.ToolTypeId)
                .NotEmpty().WithMessage("Tool type ID must be provided.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty().WithMessage("Tool status ID must be provided.");

            RuleFor(x => x.SerialNumber)
                .MaximumLength(100).WithMessage("Serial number must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));
        }
    }
}