using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface ITrainingProgressService
{
    /// <summary>Udemy training progress for a single employee, grouped/aggregated.</summary>
    Task<TrainingProgressSummaryDto> GetForUserAsync(int userId, CancellationToken cancellationToken = default);
}
