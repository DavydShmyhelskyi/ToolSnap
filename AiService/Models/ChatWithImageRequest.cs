namespace AiService.Models
{
    public class ChatWithImageRequest
    {
        public string? Prompt { get; set; }
        public IFormFile? Image { get; set; }
    }
}
