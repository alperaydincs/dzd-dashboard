using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;

namespace DZDDashboard.Client.Services;

public interface ITrainingClientService
{
    Task<TrainingProgressSummaryDto?> GetMyProgressAsync();
    Task<TrainingProgressSummaryDto?> GetUserProgressAsync(int userId);
}

public class TrainingClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), ITrainingClientService
{
    public async Task<TrainingProgressSummaryDto?> GetMyProgressAsync()
        => await GetAsync<TrainingProgressSummaryDto>(ApiRoutes.Trainings.MyProgress);

    public async Task<TrainingProgressSummaryDto?> GetUserProgressAsync(int userId)
        => await GetAsync<TrainingProgressSummaryDto>(ApiRoutes.Trainings.UserProgress(userId));
}
