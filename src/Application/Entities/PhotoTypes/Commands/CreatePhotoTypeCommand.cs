using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoTypes.Exceptions;
using Domain.Models.ToolPhotos;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoTypes.Commands
{
    public record CreatePhotoTypeCommand : IRequest<Either<PhotoTypeException, PhotoType>>
    {
        public required string Title { get; init; }
    }

    public class CreatePhotoTypeCommandHandler(
        IPhotoTypeQueries queries,
        IPhotoTypeRepository repository)
        : IRequestHandler<CreatePhotoTypeCommand, Either<PhotoTypeException, PhotoType>>
    {
        public async Task<Either<PhotoTypeException, PhotoType>> Handle(
            CreatePhotoTypeCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Title, cancellationToken);

            return await existing.MatchAsync(
                pt => new PhotoTypeAlreadyExistsException(pt.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<PhotoTypeException, PhotoType>> CreateEntity(
            CreatePhotoTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newPhotoType = PhotoType.New(request.Title);
                var result = await repository.AddAsync(newPhotoType, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledPhotoTypeException(PhotoTypeId.Empty(), ex);
            }
        }
    }
}