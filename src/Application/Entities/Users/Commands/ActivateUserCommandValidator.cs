using FluentValidation;

namespace Application.Entities.Users.Commands
{
    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID must be provided.");
        }
    }
}