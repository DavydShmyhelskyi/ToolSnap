using Domain.Models.ToolInfo;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ModelControllerService(IModelQueries modelQueries) : IModelControllerService
    {
        public async Task<Option<ModelDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await modelQueries.GetByIdAsync(new ModelId(id), cancellationToken);

            return entity.Match(
                m => ModelDto.FromDomain(m),
                () => Option<ModelDto>.None);
        }
    }
}