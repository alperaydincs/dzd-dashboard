using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface ICareerPathService
{
    Task<List<CareerPathDto>> GetCareerPathsAsync(CancellationToken cancellationToken = default);
    Task<CareerPathDto>       CreateCareerPathAsync(CareerPathDto dto, CancellationToken cancellationToken = default);
    Task                      UpdateCareerPathAsync(CareerPathDto dto, CancellationToken cancellationToken = default);
    Task                      DeleteCareerPathAsync(int id, CancellationToken cancellationToken = default);
    Task<CareerPathRuleDto>   CreateCareerPathRuleAsync(CareerPathRuleDto dto, CancellationToken cancellationToken = default);
    Task                      UpdateCareerPathRuleAsync(CareerPathRuleDto dto, CancellationToken cancellationToken = default);
    Task                      DeleteCareerPathRuleAsync(int id, CancellationToken cancellationToken = default);
}
