using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Tools.Exceptions;
using Domain.Models.ToolInfo;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Entities.Tools.Commands
{
    public record CreateToolCommand : IRequest<Either<ToolException, Tool>>
    {
        public required Guid ToolTypeId { get; init; }
        public required Guid ToolStatusId { get; init; }
        public Guid? BrandId { get; init; }
        public Guid? ModelId { get; init; }
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
            var toolTypeId = new ToolTypeId(request.ToolTypeId);
            var existing = await queries.GetByToolTypeIdAsync(toolTypeId, cancellationToken);

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
                var toolTypeId = new ToolTypeId(request.ToolTypeId);
                var toolStatusId = new ToolStatusId(request.ToolStatusId);

                BrandId? brandId =
                    request.BrandId is null ? null : new BrandId(request.BrandId.Value);

                ModelId? modelId =
                    request.ModelId is null ? null : new ModelId(request.ModelId.Value);

                var newTool = Tool.New(
                    toolTypeId,
                    toolStatusId,
                    brandId,
                    modelId,
                    request.SerialNumber);

                return await repository.AddAsync(newTool, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolException(ToolId.Empty(), ex);
            }
        }

    }
}