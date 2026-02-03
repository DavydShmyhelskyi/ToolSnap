using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoTypes.Exceptions;
using Domain.Models.ToolPhotos;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoTypes.Commands
{/*
    public record UpdatePhotoTypeCommand : IRequest<Either<PhotoTypeException, PhotoType>>
    {
        public required Guid PhotoTypeId { get; init; }
        public required string Title { get; init; }
    }

    public class UpdatePhotoTypeCommandHandler(
        IPhotoTypeQueries queries,
        IPhotoTypeRepository repository)
        : IRequestHandler<UpdatePhotoTypeCommand, Either<PhotoTypeException, PhotoType>>
    {
        public async Task<Either<PhotoTypeException, PhotoType>> Handle(
            UpdatePhotoTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new PhotoTypeId(request.PhotoTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                pt => UpdateEntity(pt, request, cancellationToken),
                () => new PhotoTypeNotFoundException(id));
        }

        private async Task<Either<PhotoTypeException, PhotoType>> UpdateEntity(
            PhotoType entity,
            UpdatePhotoTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.ChangeTitle(request.Title);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledPhotoTypeException(entity.Id, ex);
            }
        }
    }*/
}