using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class ReturnToolAssignmentsCommandItemValidator
        : AbstractValidator<ReturnToolAssignmentsCommandItem>
    {
        public ReturnToolAssignmentsCommandItemValidator()
        {
            RuleFor(x => x.ToolAssignmentId)
                .NotEmpty()
                .WithMessage("ToolAssignmentId is required.");

            RuleFor(x => x.LocationId)
                .NotEmpty()
                .WithMessage("LocationId is required.");

            RuleFor(x => x.ReturnedDetectedToolId)
                .NotEmpty()
                .WithMessage("ReturnedDetectedToolId is required.");
        }
    }
}