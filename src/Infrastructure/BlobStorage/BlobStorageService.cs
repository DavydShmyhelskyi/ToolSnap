using Application.Common.Interfaces;
using LanguageExt;

namespace Infrastructure.BlobStorage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath = "uploads";

    public async Task<Unit> UploadAsync(Stream stream, string fileFullPath, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_rootPath, fileFullPath);

        var directory = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);

        using var fileStream = new FileStream(fullPath, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken);

        return Unit.Default;
    }

    public Task<Unit> DeleteAsync(string fileFullPath, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_rootPath, fileFullPath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.FromResult(Unit.Default);
    }
    public async Task<byte[]> ReadAsync(string fileFullPath, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_rootPath, fileFullPath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"File not found: {fileFullPath}");

        await using var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        using var memoryStream = new MemoryStream();

        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}
