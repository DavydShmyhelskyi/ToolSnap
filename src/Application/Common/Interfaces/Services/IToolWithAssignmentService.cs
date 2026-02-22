using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models.Tools;
using LanguageExt;

namespace Application.Common.Interfaces.Services
{
    public interface IToolWithAssignmentService
    {
        Task<Either<Exception, Tool>> CreateToolWithAssignmentAsync(
            Guid userId,
            Guid actionTypeId,
            Guid photoTypeId,
            Guid locationId,
            Guid toolTypeId,
            Guid toolStatusId,
            Guid? brandId,
            Guid? modelId,
            string? serialNumber,
            string originalName,
            Stream fileStream,
            CancellationToken cancellationToken);
    }
}