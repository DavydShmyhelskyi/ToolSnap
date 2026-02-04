using FluentValidation;

namespace Application.Entities.Brands.Commands
{
    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandCommandValidator()
        {
            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Brand ID must be provided.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Brand title is required.")
                .MaximumLength(100).WithMessage("Brand title must not exceed 100 characters.");
        }
    }
}