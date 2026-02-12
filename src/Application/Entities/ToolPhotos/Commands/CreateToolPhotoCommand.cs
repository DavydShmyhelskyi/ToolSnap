using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolPhotos.Exceptions;
using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolPhotos.Commands
{
    public record CreateToolPhotoCommand : IRequest<Either<ToolPhotoException, ToolPhoto>>
    {
        public required Guid ToolId { get; init; }
        public required Guid PhotoTypeId { get; init; }
        public required string OriginalName { get; init; }
    }

    public class CreateToolPhotoCommandHandler(
        IToolPhotosRepository repository,
        IToolsQueries toolsQueries,
        IPhotoTypeQueries photoTypeQueries)
        : IRequestHandler<CreateToolPhotoCommand, Either<ToolPhotoException, ToolPhoto>>
    {
        public async Task<Either<ToolPhotoException, ToolPhoto>> Handle(
            CreateToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
            var toolId = new ToolId(request.ToolId);
            var photoTypeId = new PhotoTypeId(request.PhotoTypeId);
            
            // Перевірка існування Tool
            var tool = await toolsQueries.GetByIdAsync(toolId, cancellationToken);
            if (tool.IsNone)
                return new ToolNotFoundForToolPhotoException(toolId);

            // Перевірка існування PhotoType
            var photoType = await photoTypeQueries.GetByIdAsync(photoTypeId, cancellationToken);
            if (photoType.IsNone)
                return new PhotoTypeNotFoundForToolPhotoException(photoTypeId);

            return await CreateEntity(request, cancellationToken);
        }

        private async Task<Either<ToolPhotoException, ToolPhoto>> CreateEntity(
            CreateToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolId = new ToolId(request.ToolId);
                var photoTypeId = new PhotoTypeId(request.PhotoTypeId);

                var newToolPhoto = ToolPhoto.New(
                    toolId,
                    photoTypeId,
                    request.OriginalName);

                var result = await repository.AddAsync(newToolPhoto, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledToolPhotoException(ToolPhotoId.Empty(), ex);
            }
        }
    }
}