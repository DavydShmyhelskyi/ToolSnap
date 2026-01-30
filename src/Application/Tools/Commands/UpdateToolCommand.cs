using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Tools.Exceptions;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Tools.Commands
{
    public record UpdateToolCommand : IRequest<Either<ToolException, Tool>>
    {
        public required Guid ToolId { get; init; }
        public required Guid ToolStatusId { get; init; }
    }

    public class UpdateToolCommandHandler(
        IToolsQueries queries,
        IToolsRepository repository)
        : IRequestHandler<UpdateToolCommand, Either<ToolException, Tool>>
    {
        public async Task<Either<ToolException, Tool>> Handle(
            UpdateToolCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolId(request.ToolId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                t => UpdateEntity(t, request, cancellationToken),
                () => new ToolNotFoundException(id));
        }

        private async Task<Either<ToolException, Tool>> UpdateEntity(
            Tool entity,
            UpdateToolCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolStatusId = new ToolStatusId(request.ToolStatusId);
                entity.ChangeStatus(toolStatusId);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolException(entity.Id, ex);
            }
        }
    }
}