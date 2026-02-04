using Domain.Models.ToolAssignments;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ToolAssignmentControllerService(IToolAssignmentQueries toolAssignmentQueries) : IToolAssignmentControllerService
    {
        public async Task<Option<ToolAssignmentDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await toolAssignmentQueries.GetByIdAsync(new ToolAssignmentId(id), cancellationToken);

            return entity.Match(
                t => ToolAssignmentDto.FromDomain(t),
                () => Option<ToolAssignmentDto>.None);
        }
    }
}