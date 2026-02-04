using Domain.Models.ToolInfo;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class BrandControllerService(IBrandQueries brandQueries) : IBrandControllerService
    {
        public async Task<Option<BrandDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await brandQueries.GetByIdAsync(new BrandId(id), cancellationToken);

            return entity.Match(
                b => BrandDto.FromDomain(b),
                () => Option<BrandDto>.None);
        }
    }
}