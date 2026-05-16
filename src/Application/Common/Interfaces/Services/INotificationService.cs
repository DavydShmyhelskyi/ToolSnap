namespace Application.Common.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendPushAsync(string? fcmToken, string title, string body, CancellationToken cancellationToken);
    }
}
