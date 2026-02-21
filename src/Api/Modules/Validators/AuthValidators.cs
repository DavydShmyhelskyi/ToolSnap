using FluentValidation;
using Api.DTOs;

namespace Api.Modules.Validators
{
    // 🔹 LOGIN
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }

    // 🔹 REGISTER
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MinimumLength(2).WithMessage("Full name must be at least 2 characters long.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.RoleId)
                .NotEqual(Guid.Empty).When(x => x.RoleId.HasValue)
                .WithMessage("RoleId cannot be an empty GUID.");
        }
    }

    // 🔹 REFRESH TOKEN
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");
        }
    }

    // 🔹 AUTH RESPONSE (не обов’язково, але залишимо базовий контроль)
    public class AuthenticationResponseDtoValidator : AbstractValidator<AuthenticationResponseDto>
    {
        public AuthenticationResponseDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("User Id cannot be empty.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName cannot be empty.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("AccessToken cannot be empty.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken cannot be empty.");
        }
    }
}