using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Domain.Models.ToolTransfers;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ToolTransferControllerService(IToolTransferQueries toolTransferQueries) : IToolTransferControllerService
    {
        public async Task<Option<ToolTransferDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await toolTransferQueries.GetByIdAsync(new ToolTransferId(id), cancellationToken);

            return entity.Match(
                t => ToolTransferDto.FromDomain(t),
                () => Option<ToolTransferDto>.None);
        }
    }
}
