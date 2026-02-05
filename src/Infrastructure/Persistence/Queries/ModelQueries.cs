using Application.Common.Interfaces.Queries;
using Domain.Models.ToolInfo;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ModelQueries(ApplicationDbContext context) : IModelQueries
    {
        public async Task<IReadOnlyList<Model>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Models.ToListAsync(cancellationToken);
        }

        public async Task<Option<Model>> GetByIdAsync(ModelId modelId, CancellationToken cancellationToken)
        {
            var model = await context.Models.FirstOrDefaultAsync(m => m.Id == modelId, cancellationToken);
            return model != null ? Option<Model>.Some(model) : Option<Model>.None;
        }

        public async Task<Option<Model>> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var model = await context.Models.FirstOrDefaultAsync(m => m.Title == title, cancellationToken);
            return model != null ? Option<Model>.Some(model) : Option<Model>.None;
        }
    }
}