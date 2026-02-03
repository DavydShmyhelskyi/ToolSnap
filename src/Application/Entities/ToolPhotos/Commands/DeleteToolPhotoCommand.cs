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
        IToolPhotosRepository repository)
        : IRequestHandler<DeleteToolPhotoCommand, Either<ToolPhotoException, ToolPhoto>>
    {
        public async Task<Either<ToolPhotoException, ToolPhoto>> Handle(
            DeleteToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolPhotoId(request.ToolPhotoId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<ToolPhotoException, ToolPhoto>>(
                tp => repository.DeleteAsync(tp, cancellationToken).Result,
                () => new ToolPhotoNotFoundException(id));
        }
    }
}