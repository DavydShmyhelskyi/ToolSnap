using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Entities.PhotoSessions.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoSessions.Commands;

public record GetPhotoFilesBySessionCommand
    : IRequest<Either<PhotoSessionException, IReadOnlyList<(string FileName, byte[] Content)>>>
{
    public required Guid PhotoSessionId { get; init; }
}

public class GetPhotoFilesBySessionCommandHandler(
    IPhotoForDetectionQueries photoQueries,
    IFileStorage fileStorage)
    : IRequestHandler<GetPhotoFilesBySessionCommand,
        Either<PhotoSessionException, IReadOnlyList<(string FileName, byte[] Content)>>>
{
    public async Task<Either<PhotoSessionException, IReadOnlyList<(string FileName, byte[] Content)>>> Handle(
        GetPhotoFilesBySessionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sessionId = new PhotoSessionId(request.PhotoSessionId);
            var photos = await photoQueries.GetByPhotoSessionIdAsync(sessionId, cancellationToken);

            if (!photos.Any())
                return new PhotoSessionNotFoundException(sessionId);

            var files = new List<(string FileName, byte[] Content)>();

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
            return new UnhandledPhotoSessionException(
                new PhotoSessionId(request.PhotoSessionId), ex);
        }
    }
}
