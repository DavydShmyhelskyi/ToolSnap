using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Tools.Exceptions;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Entities.Tools.Commands
{
    public record DeleteToolCommand : IRequest<Either<ToolException, Tool>>
    {
        public required Guid ToolId { get; init; }
    }

    public class DeleteToolCommandHandler(
        IToolsQueries queries,
        IToolsRepository repository)
        : IRequestHandler<DeleteToolCommand, Either<ToolException, Tool>>
    {
        public async Task<Either<ToolException, Tool>> Handle(
            DeleteToolCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolId(request.ToolId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<ToolException, Tool>>(
                t => repository.DeleteAsync(t, cancellationToken).Result,
                () => new ToolNotFoundException(id));
        }
    }
}