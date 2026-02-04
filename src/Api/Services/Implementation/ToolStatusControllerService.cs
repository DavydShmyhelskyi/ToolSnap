using Domain.Models.Tools;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ToolStatusControllerService(IToolStatusQueries toolStatusQueries) : IToolStatusControllerService
    {
        public async Task<Option<ToolStatusDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await toolStatusQueries.GetByIdAsync(new ToolStatusId(id), cancellationToken);

            return entity.Match(
                t => ToolStatusDto.FromDomain(t),
                () => Option<ToolStatusDto>.None);
        }
    }
}