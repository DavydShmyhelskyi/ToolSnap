using Application.Common.Interfaces.Queries;
using Domain.Models.ToolInfo;

namespace Api.Services.GemeniAiService.Models
{
    public class DetectionPromptBuilder
    {
        private readonly IBrandQueries _brandQueries;
        private readonly IModelQueries _modelQueries;
        private readonly IToolTypeQueries _toolTypeQueries;

        public DetectionPromptBuilder(
            IBrandQueries brandQueries,
            IModelQueries modelQueries,
            IToolTypeQueries toolTypeQueries)
        {
            _brandQueries = brandQueries;
            _modelQueries = modelQueries;
            _toolTypeQueries = toolTypeQueries;
        }

        public async Task<string> BuildDetectionPromptAsync(
            CancellationToken cancellationToken)
        {
            var brands = await _brandQueries.GetAllAsync(cancellationToken);
            var models = await _modelQueries.GetAllAsync(cancellationToken);
            var toolTypes = await _toolTypeQueries.GetAllAsync(cancellationToken);

            var brandList = string.Join(", ", brands.Select(b => b.Title));
            var modelList = string.Join(", ", models.Select(m => m.Title));
            var toolTypeList = string.Join(", ", toolTypes.Select(t => t.Title));

            var systemPrompt =
$@"Detect tools in the provided images.

Available Brands: {brandList}
Available Models: {modelList}
Available Tool Types: {toolTypeList}

For each detected tool:
- Select toolType ONLY from Available Tool Types.
- Select brand ONLY from Available Brands (or null).
- Select model EXACTLY from Available Models.
  Return the full model name exactly as written.
  Do not modify or split it. If no exact match is visible, return null.
- Set confidence from 0 to 1.
- Set redFlagged to true if serious damage or safety issues are visible, otherwise false.";

            return systemPrompt;
        }
    }
}