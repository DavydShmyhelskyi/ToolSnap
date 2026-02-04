using Domain.Models.PhotoSessions;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class PhotoForDetectionControllerService(IPhotoForDetectionQueries photoForDetectionQueries) : IPhotoForDetectionControllerService
    {
        public async Task<Option<PhotoForDetectionDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await photoForDetectionQueries.GetByIdAsync(new PhotoForDetectionId(id), cancellationToken);

            return entity.Match(
                p => PhotoForDetectionDto.FromDomain(p),
                () => Option<PhotoForDetectionDto>.None);
        }
    }
}