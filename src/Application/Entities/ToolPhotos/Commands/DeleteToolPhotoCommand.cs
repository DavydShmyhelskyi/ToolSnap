using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolPhotos.Exceptions;
using Domain.Models.ToolPhotos;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolPhotos.Commands
{
    public record DeleteToolPhotoCommand : IRequest<Either<ToolPhotoException, ToolPhoto>>
    {
        public required Guid ToolPhotoId { get; init; }
    }

    public class DeleteToolPhotoCommandHandler(
        IToolPhotosQueries queries,
        IToolPhotosRepository repository,
        IFileStorage fileStorage)
        : IRequestHandler<DeleteToolPhotoCommand, Either<ToolPhotoException, ToolPhoto>>
    {
        public async Task<Either<ToolPhotoException, ToolPhoto>> Handle(
            DeleteToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolPhotoId(request.ToolPhotoId);
            var entityOption = await queries.GetByIdAsync(id, cancellationToken);

            return await entityOption.MatchAsync(
                async tp =>
                {
                    try
                    {
                        // 1️⃣ Видаляємо файл
                        await fileStorage.DeleteAsync(
                            tp.GetFilePath(),
                            cancellationToken);

                        // 2️⃣ Потім видаляємо запис з БД
                        await repository.DeleteAsync(tp, cancellationToken);

                        return tp;
                    }
                    catch (Exception ex)
                    {
                        return new UnhandledToolPhotoException(id, ex);
                    }
                },
                () => Task.FromResult<Either<ToolPhotoException, ToolPhoto>>(
                    new ToolPhotoNotFoundException(id))
            );
        }
    }
}