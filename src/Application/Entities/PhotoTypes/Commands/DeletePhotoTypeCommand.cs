using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoTypes.Exceptions;
using Domain.Models.ToolPhotos;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoTypes.Commands
{
    public record DeletePhotoTypeCommand : IRequest<Either<PhotoTypeException, PhotoType>>
    {
        public required Guid PhotoTypeId { get; init; }
    }

    public class DeletePhotoTypeCommandHandler(
        IPhotoTypeQueries queries,
        IPhotoTypeRepository repository)
        : IRequestHandler<DeletePhotoTypeCommand, Either<PhotoTypeException, PhotoType>>
    {
        public async Task<Either<PhotoTypeException, PhotoType>> Handle(
            DeletePhotoTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new PhotoTypeId(request.PhotoTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<PhotoTypeException, PhotoType>>(
                pt => repository.DeleteAsync(pt, cancellationToken).Result,
                () => new PhotoTypeNotFoundException(id));
        }
    }
}