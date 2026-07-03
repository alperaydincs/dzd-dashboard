using DZDDashboard.Common.DTOs;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class TrainingProgressService(AppDbContext context) : ITrainingProgressService
{
    public async Task<TrainingProgressSummaryDto> GetForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var rows = await context.UdemyCourseActivities.AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.LastAccessedDate)
            .ThenBy(a => a.CourseTitle)
            .ToListAsync(cancellationToken);

        var items = rows.Select(Map).ToList();

        return new TrainingProgressSummaryDto
        {
            TotalCount        = items.Count,
            CompletedCount    = items.Count(i => i.Status == TrainingProgressStatus.Completed),
            InProgressCount   = items.Count(i => i.Status == TrainingProgressStatus.InProgress),
            UpcomingCount     = items.Count(i => i.Status == TrainingProgressStatus.Upcoming),
            CompletionPercent = items.Count == 0
                ? 0
                : (int)Math.Round(items.Count(i => i.Status == TrainingProgressStatus.Completed) * 100.0 / items.Count),
            IsLinked     = items.Count > 0,
            LastSyncedAt = rows.Count == 0 ? null : rows.Max(a => a.LastSyncedAt),
            Items        = items
        };
    }

    private static TrainingProgressItemDto Map(UdemyCourseActivity a)
    {
        var percent = (int)Math.Round(Math.Clamp(a.CompletionRatio, 0, 100));
        return new TrainingProgressItemDto
        {
            CourseId         = a.CourseId,
            CourseTitle      = a.CourseTitle,
            CourseCategory   = a.CourseCategory,
            ProgressPercent  = percent,
            Status           = ResolveStatus(a, percent),
            CompletionDate   = a.CompletionDate,
            LastAccessedDate = a.LastAccessedDate,
            IsAssigned       = a.IsAssigned,
            AssignedBy       = a.AssignedBy
        };
    }

    private static TrainingProgressStatus ResolveStatus(UdemyCourseActivity a, int percent)
    {
        if (a.CompletionDate is not null || percent >= 100)
            return TrainingProgressStatus.Completed;
        return percent <= 0
            ? TrainingProgressStatus.Upcoming
            : TrainingProgressStatus.InProgress;
    }
}
