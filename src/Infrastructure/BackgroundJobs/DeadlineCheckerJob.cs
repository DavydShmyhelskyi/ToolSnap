using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs
{
    public class DeadlineCheckerJob(
        IServiceScopeFactory scopeFactory,
        ILogger<DeadlineCheckerJob> logger) : BackgroundService
    {
        private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan ReminderLeadTime = TimeSpan.FromHours(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Deadline checker job started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RunAsync(stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "Unhandled error in deadline checker job.");
                }

                await Task.Delay(CheckInterval, stoppingToken);
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();
            var assignmentQueries = scope.ServiceProvider.GetRequiredService<IToolAssignmentQueries>();
            var usersQueries = scope.ServiceProvider.GetRequiredService<IUsersQueries>();
            var repository = scope.ServiceProvider.GetRequiredService<IToolAssignmentsRepository>();
            var notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var threshold = DateTime.UtcNow.Add(ReminderLeadTime);
            var dueSoon = await assignmentQueries.GetDueSoonWithoutReminderAsync(threshold, cancellationToken);

            foreach (var assignment in dueSoon)
            {
                var userOpt = await usersQueries.GetByIdAsync(assignment.UserId, cancellationToken);
                await userOpt.IfSomeAsync(async user =>
                {
                    var minutesLeft = (int)(assignment.DueAt!.Value - DateTime.UtcNow).TotalMinutes;
                    await notifications.SendPushAsync(
                        user.FcmToken,
                        "Tool return deadline approaching",
                        $"Please return the tool assigned on {assignment.TakenAt:d}. Due in ~{minutesLeft} minutes.",
                        cancellationToken);
                });

                assignment.MarkReminderSent();
                await repository.UpdateAsync(assignment, cancellationToken);

                logger.LogInformation(
                    "Deadline reminder dispatched for assignment {AssignmentId} due at {DueAt}.",
                    assignment.Id, assignment.DueAt);
            }

            if (dueSoon.Count > 0)
                logger.LogInformation("Processed {Count} deadline reminder(s).", dueSoon.Count);
        }
    }
}
