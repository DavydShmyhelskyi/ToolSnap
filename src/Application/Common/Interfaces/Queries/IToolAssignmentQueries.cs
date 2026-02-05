using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.PhotoSessions;
using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolAssignmentQueries
    {
        Task<IReadOnlyList<ToolAssignment>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<ToolAssignment>> GetByIdAsync(ToolAssignmentId toolId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolAssignment>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolAssignment>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken);
        Task<Option<ToolAssignment>> GetLastByToolAsync(ToolId toolId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolAssignment>> GetAllByPhotoSessionAsync(PhotoSessionId locationId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolAssignment>> GetByRedFlaggedTakenDetectedToolAsync(DetectedToolId takenDetectedToolId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolAssignment>> GetByRedFlaggeReturnedDetectedToolAsync(DetectedToolId returnedDetectedToolId, CancellationToken cancellationToken);
        Task<Option<ToolAssignment>> GetLastByRedFlaggedTakenDetectedToolAsync(DetectedToolId takenDetectedToolId, CancellationToken cancellationToken);
        Task<Option<ToolAssignment>> GetLastByRedFlaggeReturnedDetectedToolAsync(DetectedToolId returnedDetectedToolId, CancellationToken cancellationToken);
    }
}
