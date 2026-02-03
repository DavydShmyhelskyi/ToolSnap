using Domain.Models.Locations;
using Domain.Models.ToolInfo;
using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolsQueries
    {
        Task<IReadOnlyList<Tool>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Tool>> GetByIdAsync(ToolId toolPhotoId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken);
        Task<Option<Tool>> GetByToolTypeIdAsync(ToolTypeId toolTypeId, CancellationToken cancellationToken);
        Task<Option<Tool>> GetByBrandAsync(string brand, CancellationToken cancellationToken);
    }
}
