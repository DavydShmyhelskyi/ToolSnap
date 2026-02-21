using Application.Common.Interfaces.Queries;
using Application.Entities.PhotoForDetections.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoForDetections.Queries;

public record GetPhotosByPhotoSessionQuery
    : IRequest<Either<PhotoForDetectionException, IReadOnlyList<PhotoForDetection>>>
{
    public required Guid PhotoSessionId { get; init; }
}

public class GetPhotosByPhotoSessionQueryHandler(
    IPhotoForDetectionQueries queries)
    : IRequestHandler<GetPhotosByPhotoSessionQuery,
        Either<PhotoForDetectionException, IReadOnlyList<PhotoForDetection>>>
{
    public async Task<Either<PhotoForDetectionException, IReadOnlyList<PhotoForDetection>>> Handle(
        GetPhotosByPhotoSessionQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var photoSessionId = new PhotoSessionId(request.PhotoSessionId);

            var photos = await queries.GetByPhotoSessionIdAsync(
                photoSessionId,
                cancellationToken);

            return Prelude.Right<PhotoForDetectionException, IReadOnlyList<PhotoForDetection>>(photos);
        }
        catch (Exception ex)
        {
            return new UnhandledPhotoForDetectionException(
                PhotoForDetectionId.Empty(),
                ex);
        }
    }
}
