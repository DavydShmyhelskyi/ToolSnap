using LanguageExt;

namespace Application.Common.Interfaces;

public interface IFileStorage
{
    Task<Unit> UploadAsync(Stream stream, string fileFullPath, CancellationToken cancellationToken);
    Task<Unit> DeleteAsync(string fileFullPath, CancellationToken cancellationToken);
    Task<byte[]> ReadAsync(string fileFullPath, CancellationToken cancellationToken);
}