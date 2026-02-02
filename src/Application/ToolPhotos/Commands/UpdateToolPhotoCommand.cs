using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.ToolPhotos.Exceptions;
using Domain.Models.ToolPhotos;
using LanguageExt;
using MediatR;

namespace Application.ToolPhotos.Commands
{
    public record UpdateToolPhotoCommand : IRequest<Either<ToolPhotoException, ToolPhoto>>
    {
        public required Guid ToolPhotoId { get; init; }
        public required Guid PhotoTypeId { get; init; }
    }

    public class UpdateToolPhotoCommandHandler(
        IToolPhotosQueries queries,
        IToolPhotosRepository repository)
        : IRequestHandler<UpdateToolPhotoCommand, Either<ToolPhotoException, ToolPhoto>>
    {
        public async Task<Either<ToolPhotoException, ToolPhoto>> Handle(
            UpdateToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolPhotoId(request.ToolPhotoId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                tp => UpdateEntity(tp, request, cancellationToken),
                () => new ToolPhotoNotFoundException(id));
        }

        private async Task<Either<ToolPhotoException, ToolPhoto>> UpdateEntity(
            ToolPhoto entity,
            UpdateToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Note: ToolPhoto is immutable, so we just update repository reference
                // If you need to change PhotoTypeId, consider creating a new photo
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolPhotoException(entity.Id, ex);
            }
        }
    }
}