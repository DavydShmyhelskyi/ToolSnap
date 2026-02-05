using Application.Common.Interfaces.Queries;
using Domain.Models.PhotoSessions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ActionTypeQueries(ApplicationDbContext context) : IActionTypeQueries
    {
        public async Task<IReadOnlyList<ActionType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ActionTypes.ToListAsync(cancellationToken);
        }

        public async Task<Option<ActionType>> GetByIdAsync(ActionTypeId actionTypeId, CancellationToken cancellationToken)
        {
            var actionType = await context.ActionTypes.FirstOrDefaultAsync(at => at.Id == actionTypeId, cancellationToken);
            return actionType != null ? Option<ActionType>.Some(actionType) : Option<ActionType>.None;
        }

        public async Task<Option<ActionType>> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var actionType = await context.ActionTypes.FirstOrDefaultAsync(at => at.Title == title, cancellationToken);
            return actionType != null ? Option<ActionType>.Some(actionType) : Option<ActionType>.None;
        }
    }
}