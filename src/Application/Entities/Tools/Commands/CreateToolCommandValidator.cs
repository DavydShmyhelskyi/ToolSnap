using FluentValidation;

namespace Application.Entities.Tools.Commands
{
    public class CreateToolCommandValidator : AbstractValidator<CreateToolCommand>
    {
        public CreateToolCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tool name is required.")
                .MaximumLength(100).WithMessage("Tool name must not exceed 100 characters.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty().WithMessage("Tool status is required.");

            RuleFor(x => x.Brand)
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Brand));

            RuleFor(x => x.Model)
                .MaximumLength(100).WithMessage("Model must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Model));

            RuleFor(x => x.SerialNumber)
                .MaximumLength(50).WithMessage("Serial number must not exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));
        }
    }
}