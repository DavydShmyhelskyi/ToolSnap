namespace AiService.Services;

/// <summary>
/// Service for analyzing photos in a session using tool type, brand, and model data
/// </summary>
public interface IPhotoAnalysisService
{
    /// <summary>
    /// Analyzes all photos in a session and returns detected tools with matched database entities
    /// </summary>
    Task<PhotoSessionAnalysisResult> AnalyzePhotoSessionAsync(
        Guid photoSessionId,
        CancellationToken cancellationToken = default);
}

public class PhotoSessionAnalysisResult
{
    public bool Success { get; set; }
    public List<AnalyzedPhotoResult> AnalyzedPhotos { get; set; } = [];
    public string? Error { get; set; }
    public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
}

public class AnalyzedPhotoResult
{
    public Guid PhotoId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public List<DetectedToolWithMetadata> DetectedTools { get; set; } = [];
    public bool HasErrors { get; set; }
    public string? ErrorMessage { get; set; }
}

public class DetectedToolWithMetadata
{
    public required string ToolType { get; set; }
    public Guid? ToolTypeId { get; set; }
    
    public string? Brand { get; set; }
    public Guid? BrandId { get; set; }
    
    public string? Model { get; set; }
    public Guid? ModelId { get; set; }
    
    public string? SerialNumber { get; set; }
    public float Confidence { get; set; }
    public bool RequiresReview { get; set; }
}