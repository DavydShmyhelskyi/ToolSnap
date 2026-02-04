using FluentValidation;

namespace Application.Entities.ToolTypes.Commands
{
    public class DeleteToolTypeCommandValidator : AbstractValidator<DeleteToolTypeCommand>
    {
        public DeleteToolTypeCommandValidator()
        {
            RuleFor(x => x.ToolTypeId)
                .NotEmpty().WithMessage("Tool type ID must be provided.");
        }
    }
}