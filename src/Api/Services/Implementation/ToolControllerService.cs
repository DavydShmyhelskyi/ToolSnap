using Domain.Models.Tools;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ToolControllerService(IToolsQueries toolsQueries) : IToolControllerService
    {
        public async Task<Option<ToolDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await toolsQueries.GetByIdAsync(new ToolId(id), cancellationToken);

            return entity.Match(
                t => ToolDto.FromDomain(t),
                () => Option<ToolDto>.None);
        }
    }
}