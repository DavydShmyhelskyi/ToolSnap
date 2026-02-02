using FluentValidation;

namespace Application.Entities.Users.Commands
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID must be provided.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("New password must not exceed 100 characters.")
                .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from the current password.");
        }
    }
}