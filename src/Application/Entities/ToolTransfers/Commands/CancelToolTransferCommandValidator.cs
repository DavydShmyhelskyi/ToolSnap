using FluentValidation;

namespace Application.Entities.ToolTransfers.Commands
{
    public class CancelToolTransferCommandValidator : AbstractValidator<CancelToolTransferCommand>
    {
        public CancelToolTransferCommandValidator()
        {
            RuleFor(x => x.TransferId)
                .NotEmpty().WithMessage("TransferId is required.");

            RuleFor(x => x.FromUserId)
                .NotEmpty().WithMessage("FromUserId is required.");
        }
    }
}
