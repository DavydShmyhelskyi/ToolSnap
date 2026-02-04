using Domain.Models.PhotoSessions;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class ActionTypeControllerService(IActionTypeQueries actionTypeQueries) : IActionTypeControllerService
    {
        public async Task<Option<ActionTypeDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await actionTypeQueries.GetByIdAsync(new ActionTypeId(id), cancellationToken);

            return entity.Match(
                a => ActionTypeDto.FromDomain(a),
                () => Option<ActionTypeDto>.None);
        }
    }
}