using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.DetectedTools.Exceptions;
using Domain.Models.DetectedTools;
using Domain.Models.PhotoSessions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;
using Unit = LanguageExt.Unit;

namespace Application.Entities.DetectedTools.Commands
{
    public record CreateDetectedToolsCommandItem
    {
        public required Guid PhotoSessionId { get; init; }
        public required Guid ToolTypeId { get; init; }
        public Guid? BrandId { get; init; }
        public Guid? ModelId { get; init; }
        public string? SerialNumber { get; init; }
        public required float Confidence { get; init; }
        public required bool RedFlagged { get; init; }
    }

    public record CreateDetectedToolsCommand
        : IRequest<Either<DetectedToolException, IReadOnlyList<DetectedTool>>>
    {
        public required IReadOnlyList<CreateDetectedToolsCommandItem> Items { get; init; }
    }

    public class CreateDetectedToolsCommandHandler(
        IDetectedToolRepository repository,
        IPhotoSessionsQueries photoSessionQueries,
        IToolTypeQueries toolTypeQueries,
        IBrandQueries brandQueries,
        IModelQueries modelQueries)
        : IRequestHandler<CreateDetectedToolsCommand, Either<DetectedToolException, IReadOnlyList<DetectedTool>>>
    {
        public async Task<Either<DetectedToolException, IReadOnlyList<DetectedTool>>> Handle(
            CreateDetectedToolsCommand request,
            CancellationToken cancellationToken)
        {

            var created = new List<DetectedTool>(request.Items.Count);

            try
            {
                foreach (var item in request.Items)
                {
                    var validationResult = await ValidateItem(item, cancellationToken);

                    if (validationResult.IsLeft)
                        return validationResult.LeftToSeq().First(); // повертаємо першу помилку

                    var detectedTool = CreateEntity(item);
                    created.Add(detectedTool);
                }

                var result = await repository.AddRangeAsync(created, cancellationToken);
                return Right<DetectedToolException, IReadOnlyList<DetectedTool>>(result);
            }
            catch (Exception ex)
            {
                return new UnhandledDetectedToolException(DetectedToolId.Empty(), ex);
            }
        }

        private async Task<Either<DetectedToolException, Unit>> ValidateItem(
            CreateDetectedToolsCommandItem item,
            CancellationToken cancellationToken)
        {
            var photoSessionId = new PhotoSessionId(item.PhotoSessionId);
            var toolTypeId = new ToolTypeId(item.ToolTypeId);

            var photoSession = await photoSessionQueries.GetByIdAsync(photoSessionId, cancellationToken);
            if (photoSession.IsNone)
                return new PhotoSessionNotFoundForDetectedToolException(photoSessionId);

            var toolType = await toolTypeQueries.GetByIdAsync(toolTypeId, cancellationToken);
            if (toolType.IsNone)
                return new ToolTypeNotFoundForDetectedToolException(toolTypeId);

            if (item.BrandId.HasValue)
            {
                var brandId = new BrandId(item.BrandId.Value);
                var brand = await brandQueries.GetByIdAsync(brandId, cancellationToken);
                if (brand.IsNone)
                    return new BrandNotFoundForDetectedToolException(brandId);
            }

            if (item.ModelId.HasValue)
            {
                var modelId = new ModelId(item.ModelId.Value);
                var model = await modelQueries.GetByIdAsync(modelId, cancellationToken);
                if (model.IsNone)
                    return new ModelNotFoundForDetectedToolException(modelId);
            }

            return Unit.Default;
        }

        private static DetectedTool CreateEntity(CreateDetectedToolsCommandItem item)
        {
            var toolTypeId = new ToolTypeId(item.ToolTypeId);
            var photoSessionId = new PhotoSessionId(item.PhotoSessionId);
            var brandId = item.BrandId.HasValue ? new BrandId(item.BrandId.Value) : null;
            var modelId = item.ModelId.HasValue ? new ModelId(item.ModelId.Value) : null;

            return DetectedTool.New(
                toolTypeId,
                photoSessionId,
                brandId,
                modelId,
                item.SerialNumber,
                item.Confidence,
                item.RedFlagged);
        }
    }
}