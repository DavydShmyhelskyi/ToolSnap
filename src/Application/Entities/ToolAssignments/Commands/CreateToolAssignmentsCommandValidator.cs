using FluentValidation;
using System.Linq;

namespace Application.Entities.ToolAssignments.Commands
{
    public class CreateToolAssignmentsCommandValidator
        : AbstractValidator<CreateToolAssignmentsCommand>
    {
        public CreateToolAssignmentsCommandValidator()
        {
            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Items collection must not be null.")
                .NotEmpty()
                .WithMessage("Items collection must not be empty.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateToolAssignmentsCommandItemValidator());

            // (Опціонально) заборонити дублювання DetectedToolId
            RuleFor(x => x.Items)
                .Must(items => items.DistinctBy(i => i.TakenDetectedToolId).Count() == items.Count)
                .WithMessage("Items collection contains duplicate TakenDetectedToolId values.");
        }
    }
}