using Api.DTOs;
using FluentValidation;

namespace Api.Modules.Validators
{
    public class InitiateToolTransferDtoValidator : AbstractValidator<InitiateToolTransferDto>
    {
        public InitiateToolTransferDtoValidator()
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

    public class RespondToToolTransferDtoValidator : AbstractValidator<RespondToToolTransferDto>
    {
        public RespondToToolTransferDtoValidator()
        {
            RuleFor(x => x.ResponderUserId)
                .NotEmpty().WithMessage("ResponderUserId is required.");
        }
    }

    public class CancelToolTransferDtoValidator : AbstractValidator<CancelToolTransferDto>
    {
        public CancelToolTransferDtoValidator()
        {
            RuleFor(x => x.InitiatorUserId)
                .NotEmpty().WithMessage("InitiatorUserId is required.");
        }
    }
}
