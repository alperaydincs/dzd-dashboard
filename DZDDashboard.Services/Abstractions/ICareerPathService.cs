using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

/// <summary>Manages career paths and career map rules for grade progression.</summary>
public interface ICareerPathService
{
    Task<List<CareerPathDto>> GetCareerPathsAsync(CancellationToken cancellationToken = default);
    Task<CareerPathDto>       CreateCareerPathAsync(CareerPathDto dto, CancellationToken cancellationToken = default);
    Task                      UpdateCareerPathAsync(CareerPathDto dto, CancellationToken cancellationToken = default);
    Task                      DeleteCareerPathAsync(int id, CancellationToken cancellationToken = default);
    Task<CareerMapRuleDto>    CreateCareerMapRuleAsync(CareerMapRuleDto dto, CancellationToken cancellationToken = default);
    Task                      UpdateCareerMapRuleAsync(CareerMapRuleDto dto, CancellationToken cancellationToken = default);
    Task                      DeleteCareerMapRuleAsync(int id, CancellationToken cancellationToken = default);
}
