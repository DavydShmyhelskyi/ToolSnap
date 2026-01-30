using Domain.Models.Locations;
using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolPhotosQueries
    {
        Task<IReadOnlyList<ToolPhoto>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<ToolPhoto>> GetByIdAsync(ToolPhotoId toolPhotoId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolPhoto>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken);
    }
}
