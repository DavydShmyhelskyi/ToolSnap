using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Entities.ToolPhotos.Exceptions;
using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolPhotos.Commands
{
    public record GetToolPhotoFileByToolAndTypeCommand
        : IRequest<Either<ToolPhotoException, (string FileName, byte[] Content)>>
    {
        public required Guid ToolId { get; init; }
        public required string PhotoTypeTitle { get; init; }
    }

    public class GetToolPhotoFileByToolAndTypeCommandHandler(
        IToolPhotosQueries toolPhotosQueries,
        IPhotoTypeQueries photoTypeQueries,
        IFileStorage fileStorage)
        : IRequestHandler<GetToolPhotoFileByToolAndTypeCommand,
            Either<ToolPhotoException, (string FileName, byte[] Content)>>
    {
        public async Task<Either<ToolPhotoException, (string FileName, byte[] Content)>> Handle(
            GetToolPhotoFileByToolAndTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolId = new ToolId(request.ToolId);

                // 1️⃣ Знаходимо PhotoType по title
                var photoTypeOption =
                    await photoTypeQueries.GetByTitleAsync(request.PhotoTypeTitle, cancellationToken);

                if (photoTypeOption.IsNone)
                    return ("", Array.Empty<byte>());

                var photoType = photoTypeOption.First();

                // 2️⃣ Беремо ПЕРШЕ фото для Tool + PhotoType
                var photoOption = await toolPhotosQueries.GetByToolIdAndPhotoTypeIdAsync(
                    toolId,
                    photoType.Id,
                    cancellationToken);

                if (photoOption.IsNone)
                    return ("", Array.Empty<byte>());

                var photo = photoOption.First();

                // 3️⃣ Читаємо файл
                var filePath = photo.GetFilePath();
                var content = await fileStorage.ReadAsync(filePath, cancellationToken);

                return (photo.OriginalName, content);
            }
            catch (Exception ex)
            {
                return new UnhandledToolPhotoException(ToolPhotoId.Empty(), ex);
            }
        }
    }
}