using FluentValidation;

namespace Application.Entities.Users.Commands
{
    public class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
    {
        public DeactivateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID must be provided.");
        }
    }
}