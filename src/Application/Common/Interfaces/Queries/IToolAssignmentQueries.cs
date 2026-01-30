using Domain.Models.Locations;
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
    }
}
