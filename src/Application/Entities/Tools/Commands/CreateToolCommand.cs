using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Tools.Exceptions;
using Domain.Models.Tools;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Tools.Commands
{
    public record CreateToolCommand : IRequest<Either<ToolException, Tool>>
    {
        public required string Name { get; init; }
        public required Guid ToolStatusId { get; init; }
        public string? Brand { get; init; }
        public string? Model { get; init; }
        public string? SerialNumber { get; init; }
    }

    public class CreateToolCommandHandler(
        IToolsQueries queries,
        IToolsRepository repository)
        : IRequestHandler<CreateToolCommand, Either<ToolException, Tool>>
    {
        public async Task<Either<ToolException, Tool>> Handle(
            CreateToolCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Name, cancellationToken);

            return await existing.MatchAsync(
                t => new ToolAlreadyExistsException(t.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<ToolException, Tool>> CreateEntity(
            CreateToolCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolStatusId = new ToolStatusId(request.ToolStatusId);
                var newTool = Tool.New(
                    request.ToolType,
                    toolStatusId,
                    request.Brand,
                    request.Model,
                    request.SerialNumber);

                var result = await repository.AddAsync(newTool, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledToolException(ToolId.Empty(), ex);
            }
        }
    }
}