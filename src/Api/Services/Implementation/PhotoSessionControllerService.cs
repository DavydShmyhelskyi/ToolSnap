using Domain.Models.PhotoSessions;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class PhotoSessionControllerService(IPhotoSessionsQueries photoSessionsQueries) : IPhotoSessionControllerService
    {
        public async Task<Option<PhotoSessionDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await photoSessionsQueries.GetByIdAsync(new PhotoSessionId(id), cancellationToken);

            return entity.Match(
                p => PhotoSessionDto.FromDomain(p),
                () => Option<PhotoSessionDto>.None);
        }
    }
}