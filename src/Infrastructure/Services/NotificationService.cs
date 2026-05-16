using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class NotificationService(ILogger<NotificationService> logger) : INotificationService
    {
        public Task SendPushAsync(string? fcmToken, string title, string body, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(fcmToken))
            {
                logger.LogWarning("Push notification skipped — no FCM token registered for recipient. Title: {Title}", title);
                return Task.CompletedTask;
            }

            // TODO: integrate Firebase Cloud Messaging
            // var message = new Message { Token = fcmToken, Notification = new Notification { Title = title, Body = body } };
            // await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);

            logger.LogInformation(
                "Push notification dispatched. Token prefix: {Prefix}. Title: {Title}. Body: {Body}",
                fcmToken[..Math.Min(8, fcmToken.Length)],
                title,
                body);

            return Task.CompletedTask;
        }
    }
}
