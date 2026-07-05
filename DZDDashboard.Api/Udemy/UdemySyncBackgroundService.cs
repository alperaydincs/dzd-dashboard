using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DZDDashboard.Api.Udemy;

/// <summary>
/// Periodically pulls course-activity from the Udemy Business Reporting API and
/// upserts it into UdemyCourseActivities, linking each row to a local user by e-mail.
/// </summary>
public class UdemySyncBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<UdemyOptions> options,
    ILogger<UdemySyncBackgroundService> logger) : BackgroundService
{
    private readonly UdemyOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.IsConfigured)
        {
            logger.LogInformation("Udemy sync disabled or not configured; background service idle.");
            return;
        }

        var interval = TimeSpan.FromHours(Math.Max(1, _options.SyncIntervalHours));

        // Run once shortly after startup, then on the configured interval.
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SyncOnceAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Udemy sync failed; will retry next interval.");
            }

            try { await Task.Delay(interval, stoppingToken); }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task SyncOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var client = scope.ServiceProvider.GetRequiredService<IUdemyApiClient>();

        logger.LogInformation("Udemy sync started.");

        // email (normalized) -> local user id
        var emailToUserId = await db.Users
            .Where(u => u.NormalizedEmail != null)
            .Select(u => new { u.Id, u.NormalizedEmail })
            .ToDictionaryAsync(u => u.NormalizedEmail!, u => u.Id, cancellationToken);

        // (UdemyUserId, CourseId) -> existing tracked row
        var existing = await db.UdemyCourseActivities
            .ToDictionaryAsync(a => (a.UdemyUserId, a.CourseId), cancellationToken);

        var now = DateTime.UtcNow;
        var seen = 0;

        await foreach (var record in client.GetAllCourseActivityAsync(cancellationToken))
        {
            seen++;
            var email = record.UserEmail?.Trim();
            if (string.IsNullOrEmpty(email)) continue;

            var normalized = email.ToUpperInvariant();
            int? userId = emailToUserId.TryGetValue(normalized, out var id) ? id : null;

            if (existing.TryGetValue((record.UserId, record.CourseId), out var entity))
            {
                Apply(entity, record, userId, now);
            }
            else
            {
                entity = new UdemyCourseActivity
                {
                    UdemyUserId = record.UserId,
                    CourseId    = record.CourseId
                };
                Apply(entity, record, userId, now);
                db.UdemyCourseActivities.Add(entity);
                existing[(record.UserId, record.CourseId)] = entity;
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Udemy sync finished. {Count} activity rows processed.", seen);
    }

    private static void Apply(UdemyCourseActivity entity, UdemyActivityRecord record, int? userId, DateTime now)
    {
        entity.UserId           = userId;
        entity.UserEmail        = record.UserEmail ?? string.Empty;
        entity.UserExternalId   = record.UserExternalId;
        entity.CourseTitle      = record.CourseTitle ?? string.Empty;
        entity.CourseCategory   = record.CourseCategory;
        entity.CourseDurationMinutes = record.CourseDuration;
        entity.CompletionRatio  = record.CompletionRatio;
        entity.EnrollDate       = record.EnrollDate;
        entity.StartDate        = record.StartDate;
        entity.CompletionDate   = record.CompletionDate;
        entity.LastAccessedDate = record.LastAccessedDate;
        entity.IsAssigned       = record.IsAssigned;
        entity.AssignedBy       = record.AssignedBy;
        entity.LastSyncedAt     = now;
    }
}
