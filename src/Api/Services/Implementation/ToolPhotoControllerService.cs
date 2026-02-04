using Domain.Models.ToolPhotos;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ToolPhotoControllerService(IToolPhotosQueries toolPhotosQueries) : IToolPhotoControllerService
    {
        public async Task<Option<ToolPhotoDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await toolPhotosQueries.GetByIdAsync(new ToolPhotoId(id), cancellationToken);

            return entity.Match(
                t => ToolPhotoDto.FromDomain(t),
                () => Option<ToolPhotoDto>.None);
        }
    }
}