using FluentValidation;

namespace Application.Entities.ToolAssignments.Commands
{
    public class CreateToolAssignmentsCommandItemValidator
        : AbstractValidator<CreateToolAssignmentsCommandItem>
    {
        public CreateToolAssignmentsCommandItemValidator()
        {
            RuleFor(x => x.TakenDetectedToolId)
                .NotEmpty()
                .WithMessage("TakenDetectedToolId is required.");

            RuleFor(x => x.ToolId)
                .NotEmpty()
                .WithMessage("ToolId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.LocationId)
                .NotEmpty()
                .WithMessage("LocationId is required.");
        }
    }
}