using FluentValidation;

namespace Application.Entities.ToolTransfers.Commands
{
    public class InitiateToolTransferCommandValidator : AbstractValidator<InitiateToolTransferCommand>
    {
        public InitiateToolTransferCommandValidator()
        {
            RuleFor(x => x.FromUserId)
                .NotEmpty().WithMessage("FromUserId is required.");

            RuleFor(x => x.ToUserId)
                .NotEmpty().WithMessage("ToUserId is required.");

            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("ToolId is required.");

            RuleFor(x => x)
                .Must(x => x.FromUserId != x.ToUserId)
                .WithMessage("Cannot transfer a tool to yourself.");
        }
    }
}
