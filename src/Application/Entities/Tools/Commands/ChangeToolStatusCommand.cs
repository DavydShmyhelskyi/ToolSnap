using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Tools.Exceptions;
using Domain.Models.Tools;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Tools.Commands
{
    public record ChangeToolStatusCommand : IRequest<Either<ToolException, Tool>>
    {
        public required Guid ToolId { get; init; }
        public required Guid ToolStatusId { get; init; }
    }

    public class ChangeToolStatusCommandHandler(
        IToolsQueries toolsQueries,
        IToolStatusQueries toolStatusQueries,
        IToolsRepository repository)
        : IRequestHandler<ChangeToolStatusCommand, Either<ToolException, Tool>>
    {
        public async Task<Either<ToolException, Tool>> Handle(
            ChangeToolStatusCommand request,
            CancellationToken cancellationToken)
        {
            var toolId = new ToolId(request.ToolId);
            var toolStatusId = new ToolStatusId(request.ToolStatusId);

            // Перевірка існування Tool
            var tool = await toolsQueries.GetByIdAsync(toolId, cancellationToken);
            if (tool.IsNone)
                return new ToolNotFoundException(toolId);

            // Перевірка існування ToolStatus
            var toolStatus = await toolStatusQueries.GetByIdAsync(toolStatusId, cancellationToken);
            if (toolStatus.IsNone)
                return new ToolStatusNotFoundForToolException(toolStatusId);

            return await tool.MatchAsync(
                t => ChangeStatus(t, toolStatusId, cancellationToken),
                () => new ToolNotFoundException(toolId));
        }

        private async Task<Either<ToolException, Tool>> ChangeStatus(
            Tool entity,
            ToolStatusId toolStatusId,
            CancellationToken cancellationToken)
        {
            try
            {
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