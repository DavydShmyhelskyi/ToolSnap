using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Models.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Models.Commands
{
    public record CreateModelCommand : IRequest<Either<ModelException, Model>>
    {
        public required string Title { get; init; }
    }

    public class CreateModelCommandHandler(
        IModelQueries queries,
        IModelRepository repository)
        : IRequestHandler<CreateModelCommand, Either<ModelException, Model>>
    {
        public async Task<Either<ModelException, Model>> Handle(
            CreateModelCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Title, cancellationToken);

            return await existing.MatchAsync(
                m => new ModelAlreadyExistsException(m.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<ModelException, Model>> CreateEntity(
            CreateModelCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newModel = Model.New(request.Title);
                var result = await repository.AddAsync(newModel, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledModelException(ModelId.Empty(), ex);
            }
        }
    }
}