using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Entities.ToolPhotos.Exceptions;
using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolPhotos.Commands
{
    public record GetToolPhotoFilesByToolCommand
        : IRequest<Either<ToolPhotoException, IReadOnlyList<(string FileName, byte[] Content)>>>
    {
        public required Guid ToolId { get; init; }
    }

    public class GetToolPhotoFilesByToolCommandHandler(
        IToolPhotosQueries toolPhotosQueries,
        IFileStorage fileStorage)
        : IRequestHandler<GetToolPhotoFilesByToolCommand,
            Either<ToolPhotoException, IReadOnlyList<(string FileName, byte[] Content)>>>
    {
        public async Task<Either<ToolPhotoException, IReadOnlyList<(string FileName, byte[] Content)>>> Handle(
            GetToolPhotoFilesByToolCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolId = new ToolId(request.ToolId);

                // 1️⃣ Отримуємо всі ToolPhoto по інструменту
                var photos = await toolPhotosQueries.GetAllByToolAsync(toolId, cancellationToken);

                if (!photos.Any())
                    return Array.Empty<(string FileName, byte[] Content)>();

                var files = new List<(string FileName, byte[] Content)>();

                // 2️⃣ Читаємо всі файли з диска
                foreach (var photo in photos)
                {
                    var filePath = photo.GetFilePath();
                    var content = await fileStorage.ReadAsync(filePath, cancellationToken);

                    files.Add((photo.OriginalName, content));
                }

                return files;
            }
            catch (Exception ex)
            {
                return new UnhandledToolPhotoException(ToolPhotoId.Empty(), ex);
            }
        }
    }
}