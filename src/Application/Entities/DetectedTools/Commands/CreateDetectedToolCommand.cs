using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.DetectedTools.Exceptions;
using Domain.Models.PhotoSessions;
using Domain.Models.DetectedTools;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.DetectedTools.Commands
{
    public record CreateDetectedToolCommand : IRequest<Either<DetectedToolException, DetectedTool>>
    {
        public required Guid PhotoSessionId { get; init; }
        public required Guid ToolTypeId { get; init; }
        public Guid? BrandId { get; init; }
        public Guid? ModelId { get; init; }
        public string? SerialNumber { get; init; }
        public required float Confidence { get; init; }
        public required bool RedFlagged { get; init; }
    }

    public class CreateDetectedToolCommandHandler(
        IDetectedToolRepository repository,
        IPhotoSessionsQueries photoSessionQueries,
        IToolTypeQueries toolTypeQueries,
        IBrandQueries brandQueries,
        IModelQueries modelQueries)
        : IRequestHandler<CreateDetectedToolCommand, Either<DetectedToolException, DetectedTool>>
    {
        public async Task<Either<DetectedToolException, DetectedTool>> Handle(
            CreateDetectedToolCommand request,
            CancellationToken cancellationToken)
        {
            var photoSessionId = new PhotoSessionId(request.PhotoSessionId);
            var toolTypeId = new ToolTypeId(request.ToolTypeId);

            // Перевірка існування PhotoSession
            var photoSession = await photoSessionQueries.GetByIdAsync(photoSessionId, cancellationToken);
            if (photoSession.IsNone)
                return new PhotoSessionNotFoundForDetectedToolException(photoSessionId);

            // Перевірка існування ToolType
            var toolType = await toolTypeQueries.GetByIdAsync(toolTypeId, cancellationToken);
            if (toolType.IsNone)
                return new ToolTypeNotFoundForDetectedToolException(toolTypeId);

            // Перевірка існування Brand (якщо вказано)
            if (request.BrandId.HasValue)
            {
                var brandId = new BrandId(request.BrandId.Value);
                var brand = await brandQueries.GetByIdAsync(brandId, cancellationToken);
                if (brand.IsNone)
                    return new BrandNotFoundForDetectedToolException(brandId);
            }

            // Перевірка існування Model (якщо вказано)
            if (request.ModelId.HasValue)
            {
                var modelId = new ModelId(request.ModelId.Value);
                var model = await modelQueries.GetByIdAsync(modelId, cancellationToken);
                if (model.IsNone)
                    return new ModelNotFoundForDetectedToolException(modelId);
            }

            return await CreateEntity(request, cancellationToken);
        }

        private async Task<Either<DetectedToolException, DetectedTool>> CreateEntity(
            CreateDetectedToolCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolTypeId = new ToolTypeId(request.ToolTypeId);
                var photoSessionId = new PhotoSessionId(request.PhotoSessionId);
                var brandId = request.BrandId.HasValue ? new BrandId(request.BrandId.Value) : null;
                var modelId = request.ModelId.HasValue ? new ModelId(request.ModelId.Value) : null;

                var newDetectedTool = DetectedTool.New(
                    toolTypeId,
                    photoSessionId,
                    brandId,
                    modelId,
                    request.SerialNumber,
                    request.Confidence,
                    request.RedFlagged);

                var result = await repository.AddAsync(newDetectedTool, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledDetectedToolException(DetectedToolId.Empty(), ex);
            }
        }
    }
}