using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface ILookupService
{
    Task<List<LookupValueDto>> GetAsync(string category, CancellationToken cancellationToken = default);
    Task<LookupValueDto> CreateAsync(LookupValueDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, LookupValueDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string category, int id, CancellationToken cancellationToken = default);
}
