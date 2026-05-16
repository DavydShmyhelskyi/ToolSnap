using FluentValidation;

namespace Application.Entities.ToolTransfers.Commands
{
    public class AcceptToolTransferCommandValidator : AbstractValidator<AcceptToolTransferCommand>
    {
        public AcceptToolTransferCommandValidator()
        {
            RuleFor(x => x.TransferId)
                .NotEmpty().WithMessage("TransferId is required.");

            RuleFor(x => x.ToUserId)
                .NotEmpty().WithMessage("ToUserId is required.");
        }
    }
}
