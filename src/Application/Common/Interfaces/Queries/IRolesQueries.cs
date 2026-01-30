using Domain.Models.Locations;
using Domain.Models.Roles;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Queries
{
    public interface IRolesQueries
    {
        Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Role>> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken);
        Task<Option<Role>> GetByTitleAsync(string name, CancellationToken cancellationToken);
    }
}
