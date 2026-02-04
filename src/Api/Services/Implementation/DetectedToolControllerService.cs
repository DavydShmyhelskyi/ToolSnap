using Domain.Models.DetectedTools;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class DetectedToolControllerService(IDetectedToolQueries detectedToolQueries) : IDetectedToolControllerService
    {
        public async Task<Option<DetectedToolDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await detectedToolQueries.GetByIdAsync(new DetectedToolId(id), cancellationToken);

            return entity.Match(
                d => DetectedToolDto.FromDomain(d),
                () => Option<DetectedToolDto>.None);
        }
    }
}