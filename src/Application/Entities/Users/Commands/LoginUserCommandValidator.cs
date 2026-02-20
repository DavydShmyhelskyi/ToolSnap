using Application.Entities.Users.Commands;
using FluentValidation;

namespace Application.Entities.Users.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(4).WithMessage("Password must be at least 4 characters");
        RuleFor (x => x.Latitude)
            .NotEmpty()
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");
        RuleFor (x => x.Longitude)
            .NotEmpty()
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");
    }
}
