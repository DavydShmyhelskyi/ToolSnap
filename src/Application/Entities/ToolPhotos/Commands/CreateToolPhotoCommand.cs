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
        IToolPhotosRepository repository)
        : IRequestHandler<CreateToolPhotoCommand, Either<ToolPhotoException, ToolPhoto>>
    {
        public async Task<Either<ToolPhotoException, ToolPhoto>> Handle(
            CreateToolPhotoCommand request,
            CancellationToken cancellationToken)
        {
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