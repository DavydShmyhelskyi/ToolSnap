using Api.Services.GemeniAiService.Models;
using Application.Common.Interfaces.Queries;
using Application.Entities.PhotoSessions.Commands;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Api.Services.GemeniAiService;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IBrandQueries _brandQueries;
    private readonly IModelQueries _modelQueries;
    private readonly IToolTypeQueries _toolTypeQueries;
    private readonly IMediator _mediator;

    public GeminiService(
        HttpClient httpClient,
        IConfiguration configuration,
        IBrandQueries brandQueries,
        IModelQueries modelQueries,
        IToolTypeQueries toolTypeQueries,
        IMediator mediator)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _brandQueries = brandQueries;
        _modelQueries = modelQueries;
        _toolTypeQueries = toolTypeQueries;
        _mediator = mediator;
    }

    private string GetApiKey()
        => _configuration["GeminiSettings:ApiKey"]
           ?? throw new InvalidOperationException("Gemini API key is not configured");

    // ===== ЄДИНИЙ метод спілкування з Gemini =====
    public async Task<string> ChatWithImagesAsync(
        ChatWithImagesRequest request,
        CancellationToken cancellationToken = default)
    {
        var parts = new List<object>
        {
            new { text = request.Prompt }
        };

        foreach (var image in request.Images)
        {
            parts.Add(new
            {
                inline_data = new
                {
                    mime_type = image.MimeType,
                    data = Convert.ToBase64String(image.Content)
                }
            });
        }

        var body = new
        {
            contents = new[]
         {
            new { parts }
         },
            generationConfig = new
            {
                temperature = 0,
                responseMimeType = "application/json",
                responseSchema = new
                {
                    type = "object",
                    properties = new
                    {
                        detections = new
                        {
                            type = "array",
                            items = new
                            {
                                type = "object",
                                properties = new
                                {
                                    toolType = new { type = "string" },
                                    brand = new { type = "string", nullable = true },
                                    model = new { type = "string", nullable = true },
                                    confidence = new { type = "number" },
                                    redFlagged = new { type = "boolean" }
                                },
                                required = new[]
                         {
                            "toolType",
                            "confidence",
                            "redFlagged"
                        }
                            }
                        }
                    },
                    required = new[] { "detections" }
                }
            }
        };



        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent");

        httpRequest.Headers.Add("X-goog-api-key", GetApiKey());
        httpRequest.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var json = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        return json.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString()
            ?? "No response from Gemini";
    }

    // ===== Бізнес-кейс: детекція інструментів =====
    public async Task<string> DetectToolsFromSessionAsync(
        Guid photoSessionId,
        CancellationToken cancellationToken = default)
    {
        var photosResult = await _mediator.Send(
            new GetPhotoFilesBySessionCommand { PhotoSessionId = photoSessionId },
            cancellationToken);

        var photoFiles = photosResult.Match(
            Right: files => files,
            Left: ex => throw new InvalidOperationException($"Failed to retrieve photos: {ex}")
        );

        if (photoFiles.Count == 0)
            throw new InvalidOperationException("No photos found in the session");

        var promptBuilder = new DetectionPromptBuilder(
            _brandQueries,
            _modelQueries,
            _toolTypeQueries);

        var prompt = await promptBuilder.BuildDetectionPromptAsync(
            cancellationToken);

        var images = photoFiles
            .Select(p => new ImageContent
            {
                Content = p.Content,
                MimeType = GetMimeType(p.FileName),
                FileName = p.FileName
            })
            .ToList();

        var request = new ChatWithImagesRequest
        {
            Prompt = prompt,
            Images = images
        };

        return await ChatWithImagesAsync(request, cancellationToken);
    }

    private static string GetMimeType(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        return provider.TryGetContentType(fileName, out var type)
            ? type
            : "application/octet-stream";
    }
}
