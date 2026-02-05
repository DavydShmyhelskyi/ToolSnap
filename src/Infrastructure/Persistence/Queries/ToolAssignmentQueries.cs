using Application.Common.Interfaces.Queries;
using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.PhotoSessions;
using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolAssignmentQueries(ApplicationDbContext context) : IToolAssignmentQueries
    {
        public async Task<IReadOnlyList<ToolAssignment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ToolAssignments.ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolAssignment>> GetByIdAsync(ToolAssignmentId toolId, CancellationToken cancellationToken)
        {
            var toolAssignment = await context.ToolAssignments.FirstOrDefaultAsync(ta => ta.Id == toolId, cancellationToken);
            return toolAssignment != null ? Option<ToolAssignment>.Some(toolAssignment) : Option<ToolAssignment>.None;
        }

        public async Task<IReadOnlyList<ToolAssignment>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.ToolAssignments.Where(ta => ta.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolAssignment>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            return await context.ToolAssignments.Where(ta => ta.ToolId == toolId).ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolAssignment>> GetLastByToolAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            var toolAssignment = await context.ToolAssignments
                .Where(ta => ta.ToolId == toolId)
                .OrderByDescending(ta => ta.Id)
                .FirstOrDefaultAsync(cancellationToken);
            return toolAssignment != null ? Option<ToolAssignment>.Some(toolAssignment) : Option<ToolAssignment>.None;
        }

        public async Task<IReadOnlyList<ToolAssignment>> GetAllByPhotoSessionAsync(PhotoSessionId locationId, CancellationToken cancellationToken)
        {
            return await context.ToolAssignments.Where(ta => ta.PhotoSessionId == locationId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolAssignment>> GetByRedFlaggedTakenDetectedToolAsync(DetectedToolId takenDetectedToolId, CancellationToken cancellationToken)
        {
            return await context.ToolAssignments.Where(ta => ta.TakenDetectedToolId == takenDetectedToolId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolAssignment>> GetByRedFlaggeReturnedDetectedToolAsync(DetectedToolId returnedDetectedToolId, CancellationToken cancellationToken)
        {
            return await context.ToolAssignments.Where(ta => ta.ReturnedDetectedToolId == returnedDetectedToolId).ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolAssignment>> GetLastByRedFlaggedTakenDetectedToolAsync(DetectedToolId takenDetectedToolId, CancellationToken cancellationToken)
        {
            var toolAssignment = await context.ToolAssignments
                .Where(ta => ta.TakenDetectedToolId == takenDetectedToolId)
                .OrderByDescending(ta => ta.Id)
                .FirstOrDefaultAsync(cancellationToken);
            return toolAssignment != null ? Option<ToolAssignment>.Some(toolAssignment) : Option<ToolAssignment>.None;
        }

        public async Task<Option<ToolAssignment>> GetLastByRedFlaggeReturnedDetectedToolAsync(DetectedToolId returnedDetectedToolId, CancellationToken cancellationToken)
        {
            var toolAssignment = await context.ToolAssignments
                .Where(ta => ta.ReturnedDetectedToolId == returnedDetectedToolId)
                .OrderByDescending(ta => ta.Id)
                .FirstOrDefaultAsync(cancellationToken);
            return toolAssignment != null ? Option<ToolAssignment>.Some(toolAssignment) : Option<ToolAssignment>.None;
        }
    }
}
