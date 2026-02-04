using FluentValidation;

namespace Application.Entities.Brands.Commands
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Brand title is required.")
                .MaximumLength(100).WithMessage("Brand title must not exceed 100 characters.");
        }
    }
}