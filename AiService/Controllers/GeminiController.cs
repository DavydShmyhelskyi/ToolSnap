using AiService.Models;
using AiService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly GeminiService _geminiService;

        public ChatController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return BadRequest("Prompt cannot be empty.");
            }

            try
            {
                var response = await _geminiService.getChatResponse(prompt);
                return Ok(new GeminiResponse { Response = response });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Internal server error: {ex.Message}"
                );
            }
        }

        [HttpPost("chat-with-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ChatWithImage([FromForm] ChatWithImageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt) && request.Image == null)
                return BadRequest("Prompt or image required.");

            using var ms = new MemoryStream();
            if (request.Image != null)
                await request.Image.CopyToAsync(ms);

            var response = await _geminiService.GetChatResponseWithImage(
                request.Prompt,
                ms.ToArray(),
                request.Image?.ContentType
            );

            return Ok(new GeminiResponse { Response = response });
        }

    }
}
