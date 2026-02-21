using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    // CreateToolAssignmentDto
    public class CreateToolAssignmentDtoValidator : AbstractValidator<CreateToolAssignmentDto>
    {
        public CreateToolAssignmentDtoValidator()
        {
            RuleFor(x => x.TakenDetectedToolId)
                .NotEmpty().WithMessage("TakenDetectedToolId is required.");

            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("ToolId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.TakenLocationId)
                .NotEmpty().WithMessage("TakenLocationId is required.");
        }
    }

    // ReturnToolAssignmentDto
    public class ReturnToolAssignmentDtoValidator : AbstractValidator<ReturnToolAssignmentDto>
    {
        public ReturnToolAssignmentDtoValidator()
        {
            RuleFor(x => x.ReturnedLocationId)
                .NotEmpty().WithMessage("ReturnedLocationId is required.");

            RuleFor(x => x.ReturnedDetectedToolId)
                .NotEmpty().WithMessage("ReturnedDetectedToolId is required.");
        }
    }

    // Batch Item for Create
    public class CreateToolAssignmentsBatchItemDtoValidator : AbstractValidator<CreateToolAssignmentsBatchItemDto>
    {
        public CreateToolAssignmentsBatchItemDtoValidator()
        {
            RuleFor(x => x.TakenDetectedToolId)
                .NotEmpty().WithMessage("TakenDetectedToolId is required.");

            RuleFor(x => x.ToolId)
                .NotEmpty().WithMessage("ToolId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("LocationId is required.");
        }
    }

    // Batch Create
    public class CreateToolAssignmentsBatchDtoValidator : AbstractValidator<CreateToolAssignmentsBatchDto>
    {
        public CreateToolAssignmentsBatchDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items cannot be null.")
                .NotEmpty().WithMessage("Items cannot be empty.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateToolAssignmentsBatchItemDtoValidator());
        }
    }

    // Batch Item for Return
    public class ReturnToolAssignmentsBatchItemDtoValidator : AbstractValidator<ReturnToolAssignmentsBatchItemDto>
    {
        public ReturnToolAssignmentsBatchItemDtoValidator()
        {
            RuleFor(x => x.ToolAssignmentId)
                .NotEmpty().WithMessage("ToolAssignmentId is required.");

            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("LocationId is required.");

            RuleFor(x => x.ReturnedDetectedToolId)
                .NotEmpty().WithMessage("ReturnedDetectedToolId is required.");
        }
    }

    // Batch Return
    public class ReturnToolAssignmentsBatchDtoValidator : AbstractValidator<ReturnToolAssignmentsBatchDto>
    {
        public ReturnToolAssignmentsBatchDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items cannot be null.")
                .NotEmpty().WithMessage("Items cannot be empty.");

            RuleForEach(x => x.Items)
                .SetValidator(new ReturnToolAssignmentsBatchItemDtoValidator());
        }
    }
}