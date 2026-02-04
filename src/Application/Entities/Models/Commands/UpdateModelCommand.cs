using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Models.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Models.Commands
{
    public record UpdateModelCommand : IRequest<Either<ModelException, Model>>
    {
        public required Guid ModelId { get; init; }
        public required string Title { get; init; }
    }

    public class UpdateModelCommandHandler(
        IModelQueries queries,
        IModelRepository repository)
        : IRequestHandler<UpdateModelCommand, Either<ModelException, Model>>
    {
        public async Task<Either<ModelException, Model>> Handle(
            UpdateModelCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ModelId(request.ModelId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                m => UpdateEntity(m, request, cancellationToken),
                () => new ModelNotFoundException(id));
        }

        private async Task<Either<ModelException, Model>> UpdateEntity(
            Model entity,
            UpdateModelCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Update(request.Title);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledModelException(entity.Id, ex);
            }
        }
    }
}