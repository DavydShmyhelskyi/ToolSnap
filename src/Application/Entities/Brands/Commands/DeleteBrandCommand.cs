using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Brands.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Brands.Commands
{
    public record DeleteBrandCommand : IRequest<Either<BrandException, Brand>>
    {
        public required Guid BrandId { get; init; }
    }

    public class DeleteBrandCommandHandler(
        IBrandQueries queries,
        IBrandRepository repository)
        : IRequestHandler<DeleteBrandCommand, Either<BrandException, Brand>>
    {
        public async Task<Either<BrandException, Brand>> Handle(
            DeleteBrandCommand request,
            CancellationToken cancellationToken)
        {
            var id = new BrandId(request.BrandId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<BrandException, Brand>>(
                b => repository.DeleteAsync(b, cancellationToken).Result,
                () => new BrandNotFoundException(id));
        }
    }
}