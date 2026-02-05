using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Tools.Exceptions;
using Domain.Models.Tools;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.Tools.Commands
{
    public record UpdateToolCommand : IRequest<Either<ToolException, Tool>>
    {
        public required Guid ToolId { get; init; }
        public Guid? BrandId { get; init; }
        public Guid? ModelId { get; init; }
        public string? SerialNumber { get; init; }
    }

    public class UpdateToolCommandHandler(
        IToolsQueries toolsQueries,
        IBrandQueries brandQueries,
        IModelQueries modelQueries,
        IToolsRepository repository)
        : IRequestHandler<UpdateToolCommand, Either<ToolException, Tool>>
    {
        public async Task<Either<ToolException, Tool>> Handle(
            UpdateToolCommand request,
            CancellationToken cancellationToken)
        {
            var toolId = new ToolId(request.ToolId);

            // Перевірка існування Tool
            var tool = await toolsQueries.GetByIdAsync(toolId, cancellationToken);
            if (tool.IsNone)
                return new ToolNotFoundException(toolId);

            // Перевірка існування Brand (якщо вказано)
            if (request.BrandId.HasValue)
            {
                var brandId = new BrandId(request.BrandId.Value);
                var brand = await brandQueries.GetByIdAsync(brandId, cancellationToken);
                if (brand.IsNone)
                    return new BrandNotFoundForToolException(brandId);
            }

            // Перевірка існування Model (якщо вказано)
            if (request.ModelId.HasValue)
            {
                var modelId = new ModelId(request.ModelId.Value);
                var model = await modelQueries.GetByIdAsync(modelId, cancellationToken);
                if (model.IsNone)
                    return new ModelNotFoundForToolException(modelId);
            }

            return await tool.MatchAsync(
                t => UpdateEntity(t, request, cancellationToken),
                () => new ToolNotFoundException(toolId));
        }

        private async Task<Either<ToolException, Tool>> UpdateEntity(
            Tool entity,
            UpdateToolCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var brandId = request.BrandId.HasValue ? new BrandId(request.BrandId.Value) : null;
                var modelId = request.ModelId.HasValue ? new ModelId(request.ModelId.Value) : null;

                entity.Update(brandId, modelId, request.SerialNumber);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolException(entity.Id, ex);
            }
        }
    }
}