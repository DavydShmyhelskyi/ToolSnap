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
        public required Guid ToolTypeId { get; init; }
        public Guid? BrandId { get; init; }
        public Guid? ModelId { get; init; }
        public required Guid ToolStatusId { get; init; }
        public string? SerialNumber { get; init; }
    }

    public class CreateToolCommandHandler(
        IToolsRepository repository,
        IToolTypeQueries toolTypeQueries,
        IBrandQueries brandQueries,
        IModelQueries modelQueries,
        IToolStatusQueries toolStatusQueries)
        : IRequestHandler<CreateToolCommand, Either<ToolException, Tool>>
    {
        public async Task<Either<ToolException, Tool>> Handle(
            CreateToolCommand request,
            CancellationToken cancellationToken)
        {
            var toolTypeId = new ToolTypeId(request.ToolTypeId);
            var toolStatusId = new ToolStatusId(request.ToolStatusId);

            // Перевірка існування ToolType
            var toolType = await toolTypeQueries.GetByIdAsync(toolTypeId, cancellationToken);
            if (toolType.IsNone)
                return new ToolTypeNotFoundForToolException(toolTypeId);

            // Перевірка існування ToolStatus
            var toolStatus = await toolStatusQueries.GetByIdAsync(toolStatusId, cancellationToken);
            if (toolStatus.IsNone)
                return new ToolStatusNotFoundForToolException(toolStatusId);

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

            return await CreateEntity(request, cancellationToken);
        }

        private async Task<Either<ToolException, Tool>> CreateEntity(
            CreateToolCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolTypeId = new ToolTypeId(request.ToolTypeId);
                var brandId = request.BrandId.HasValue ? new BrandId(request.BrandId.Value) : null;
                var modelId = request.ModelId.HasValue ? new ModelId(request.ModelId.Value) : null;
                var toolStatusId = new ToolStatusId(request.ToolStatusId);

                var newTool = Tool.New(
                    toolTypeId,
                    brandId,
                    modelId,
                    toolStatusId,
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