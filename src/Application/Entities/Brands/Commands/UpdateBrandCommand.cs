using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Brands.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Brands.Commands
{
    public record UpdateBrandCommand : IRequest<Either<BrandException, Brand>>
    {
        public required Guid BrandId { get; init; }
        public required string Title { get; init; }
    }

    public class UpdateBrandCommandHandler(
        IBrandQueries queries,
        IBrandRepository repository)
        : IRequestHandler<UpdateBrandCommand, Either<BrandException, Brand>>
    {
        public async Task<Either<BrandException, Brand>> Handle(
            UpdateBrandCommand request,
            CancellationToken cancellationToken)
        {
            var id = new BrandId(request.BrandId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                b => UpdateEntity(b, request, cancellationToken),
                () => new BrandNotFoundException(id));
        }

        private async Task<Either<BrandException, Brand>> UpdateEntity(
            Brand entity,
            UpdateBrandCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Update(request.Title);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledBrandException(entity.Id, ex);
            }
        }
    }
}