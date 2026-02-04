using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Models.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Models.Commands
{
    public record DeleteModelCommand : IRequest<Either<ModelException, Model>>
    {
        public required Guid ModelId { get; init; }
    }

    public class DeleteModelCommandHandler(
        IModelQueries queries,
        IModelRepository repository)
        : IRequestHandler<DeleteModelCommand, Either<ModelException, Model>>
    {
        public async Task<Either<ModelException, Model>> Handle(
            DeleteModelCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ModelId(request.ModelId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<ModelException, Model>>(
                m => repository.DeleteAsync(m, cancellationToken).Result,
                () => new ModelNotFoundException(id));
        }
    }
}