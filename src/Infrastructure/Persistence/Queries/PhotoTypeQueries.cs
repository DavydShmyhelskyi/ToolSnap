using Application.Common.Interfaces.Queries;
using Domain.Models.ToolPhotos;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class PhotoTypeQueries(ApplicationDbContext context) : IPhotoTypeQueries
    {
        public async Task<IReadOnlyList<PhotoType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.PhotoTypes.ToListAsync(cancellationToken);
        }

        public async Task<Option<PhotoType>> GetByIdAsync(PhotoTypeId photoTypeId, CancellationToken cancellationToken)
        {
            var photoType = await context.PhotoTypes.FirstOrDefaultAsync(pt => pt.Id == photoTypeId, cancellationToken);
            return photoType != null ? Option<PhotoType>.Some(photoType) : Option<PhotoType>.None;
        }

        public async Task<Option<PhotoType>> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            var photoType = await context.PhotoTypes.FirstOrDefaultAsync(pt => pt.Title == name, cancellationToken);
            return photoType != null ? Option<PhotoType>.Some(photoType) : Option<PhotoType>.None;
        }
    }
}
