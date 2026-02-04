using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Brands.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Brands.Commands
{
    public record CreateBrandCommand : IRequest<Either<BrandException, Brand>>
    {
        public required string Title { get; init; }
    }

    public class CreateBrandCommandHandler(
        IBrandQueries queries,
        IBrandRepository repository)
        : IRequestHandler<CreateBrandCommand, Either<BrandException, Brand>>
    {
        public async Task<Either<BrandException, Brand>> Handle(
            CreateBrandCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Title, cancellationToken);

            return await existing.MatchAsync(
                b => new BrandAlreadyExistsException(b.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<BrandException, Brand>> CreateEntity(
            CreateBrandCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newBrand = Brand.New(request.Title);
                var result = await repository.AddAsync(newBrand, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledBrandException(BrandId.Empty(), ex);
            }
        }
    }
}