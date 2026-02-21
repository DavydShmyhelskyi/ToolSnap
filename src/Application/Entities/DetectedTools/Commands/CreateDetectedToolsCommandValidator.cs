using FluentValidation;
using System.Linq;

namespace Application.Entities.DetectedTools.Commands
{
    public class CreateDetectedToolsCommandValidator
        : AbstractValidator<CreateDetectedToolsCommand>
    {
        public CreateDetectedToolsCommandValidator()
        {
            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Items collection must not be null.")
                .NotEmpty()
                .WithMessage("Items collection must not be empty.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateDetectedToolsCommandItemValidator());

            // (Опціонально) заборонити дублікати PhotoSessionId + ToolTypeId + SerialNumber (як приклад)
            RuleFor(x => x.Items)
                .Must(items =>
                {
                    // Наприклад, вважаємо унікальною комбінацію (PhotoSessionId, ToolTypeId, SerialNumber)
                    var distinctCount = items
                        .Select(i => new { i.PhotoSessionId, i.ToolTypeId, Serial = i.SerialNumber?.Trim().ToLowerInvariant() })
                        .Distinct()
                        .Count();

                    return distinctCount == items.Count;
                })
                .WithMessage("Items collection contains duplicate detected tools for the same photo session.");
        }
    }
}