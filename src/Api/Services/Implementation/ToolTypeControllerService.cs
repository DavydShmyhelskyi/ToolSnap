using Domain.Models.ToolInfo;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ToolTypeControllerService(IToolTypeQueries toolTypeQueries) : IToolTypeControllerService
    {
        public async Task<Option<ToolTypeDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await toolTypeQueries.GetByIdAsync(new ToolTypeId(id), cancellationToken);

            return entity.Match(
                t => ToolTypeDto.FromDomain(t),
                () => Option<ToolTypeDto>.None);
        }
    }
}