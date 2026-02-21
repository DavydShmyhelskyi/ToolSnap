using Application.Common.Interfaces;
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
        public required Stream FileStream { get; init; }
    }

    public class CreateToolPhotoCommandHandler(
        IToolPhotosRepository repository,
        IToolsQueries toolsQueries,
        IPhotoTypeQueries photoTypeQueries,
        IFileStorage fileStorage)
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

            return await CreateAndUpload(request, cancellationToken);
        }

        private async Task<Either<ToolPhotoException, ToolPhoto>> CreateAndUpload(
            CreateToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
            ToolPhoto? entity = null;

            try
            {
                var toolId = new ToolId(request.ToolId);
                var photoTypeId = new PhotoTypeId(request.PhotoTypeId);

                entity = ToolPhoto.New(
                    toolId,
                    photoTypeId,
                    request.OriginalName);

                // 1️⃣ Спочатку upload файлу
                await fileStorage.UploadAsync(
                    request.FileStream,
                    entity.GetFilePath(),
                    cancellationToken);

                // 2️⃣ Потім збереження у БД
                await repository.AddAsync(entity, cancellationToken);

                return entity;
            }
            catch (Exception ex)
            {
                // Якщо впало після створення entity
                var id = entity?.Id ?? ToolPhotoId.Empty();
                return new UnhandledToolPhotoException(id, ex);
            }
        }
    }
}