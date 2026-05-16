using FluentValidation;

namespace Application.Entities.Tools.Commands
{
    public class UpdateToolCommandValidator : AbstractValidator<UpdateToolCommand>
    {
        public UpdateToolCommandValidator()
        {
            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("Tool ID must be provided.");

            RuleFor(x => x.SerialNumber)
                .MaximumLength(100).WithMessage("Serial number must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or positive.");
        }
    }
}