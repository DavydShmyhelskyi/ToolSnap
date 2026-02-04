using Domain.Models.ToolPhotos;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class PhotoTypeControllerService(IPhotoTypeQueries photoTypeQueries) : IPhotoTypeControllerService
    {
        public async Task<Option<PhotoTypeDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await photoTypeQueries.GetByIdAsync(new PhotoTypeId(id), cancellationToken);

            return entity.Match(
                p => PhotoTypeDto.FromDomain(p),
                () => Option<PhotoTypeDto>.None);
        }
    }
}