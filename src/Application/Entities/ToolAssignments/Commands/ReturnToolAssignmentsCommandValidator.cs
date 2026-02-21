using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class ReturnToolAssignmentsCommandValidator
        : AbstractValidator<ReturnToolAssignmentsCommand>
    {
        public ReturnToolAssignmentsCommandValidator()
        {
            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Items collection must not be null.")
                .NotEmpty()
                .WithMessage("Items collection must not be empty.");

            RuleForEach(x => x.Items)
                .SetValidator(new ReturnToolAssignmentsCommandItemValidator());

            // (Опціонально) перевірка на дублікати ToolAssignmentId
            RuleFor(x => x.Items)
                .Must(items => items.DistinctBy(i => i.ToolAssignmentId).Count() == items.Count)
                .WithMessage("Items collection contains duplicate ToolAssignmentId values.");
        }
    }
}