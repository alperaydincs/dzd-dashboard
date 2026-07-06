using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface ITrainingProgressService
{
    Task<TrainingProgressSummaryDto> GetForUserAsync(int userId, CancellationToken cancellationToken = default);
}
