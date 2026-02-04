using FluentValidation;

namespace Application.Entities.Brands.Commands
{
    public class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
    {
        public DeleteBrandCommandValidator()
        {
            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Brand ID must be provided.");
        }
    }
}