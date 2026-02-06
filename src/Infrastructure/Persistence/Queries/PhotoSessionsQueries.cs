using Application.Common.Interfaces.Queries;
using Domain.Models.PhotoSessions;
using Domain.Models.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class PhotoSessionsQueries(ApplicationDbContext context) : IPhotoSessionsQueries
    {
        public async Task<IReadOnlyList<PhotoSession>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.PhotoSessions.ToListAsync(cancellationToken);
        }

        public async Task<Option<PhotoSession>> GetByIdAsync(PhotoSessionId photoSessionId, CancellationToken cancellationToken)
        {
            var photoSession = await context.PhotoSessions.FirstOrDefaultAsync(ps => ps.Id == photoSessionId, cancellationToken);
            return photoSession != null ? Option<PhotoSession>.Some(photoSession) : Option<PhotoSession>.None;
        }

        public async Task<IReadOnlyList<PhotoSession>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.PhotoSessions
                .Where(ps =>
                    ps.DetectedTools.Any(dt =>
                        dt.ToolAssignment != null &&
                        dt.ToolAssignment.UserId == userId))
                .Distinct()
                .ToListAsync(cancellationToken);
        }

    }
}