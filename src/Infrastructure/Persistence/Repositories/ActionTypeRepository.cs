using Application.Common.Interfaces.Repositories;
using Domain.Models.PhotoSessions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ActionTypeRepository(ApplicationDbContext context) : IActionTypeRepository
    {
        public async Task<ActionType> AddAsync(ActionType entity, CancellationToken cancellationToken)
        {
            await context.ActionTypes.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ActionType> UpdateAsync(ActionType entity, CancellationToken cancellationToken)
        {
            context.ActionTypes.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ActionType> DeleteAsync(ActionType entity, CancellationToken cancellationToken)
        {
            context.ActionTypes.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}