namespace Api.Services.GemeniAiService.Models;

public class ChatWithImagesRequest
{
    public required string Prompt { get; init; }

    public required IReadOnlyList<ImageContent> Images { get; init; }
}

public class ImageContent
{
    public required byte[] Content { get; init; }
    public required string MimeType { get; init; }
    public string? FileName { get; init; }
}
