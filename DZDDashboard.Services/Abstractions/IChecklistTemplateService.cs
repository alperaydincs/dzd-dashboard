using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IChecklistTemplateService
{
    Task<List<ChecklistStepTemplateDto>> GetAsync(string processType, CancellationToken cancellationToken = default);
    Task<ChecklistStepTemplateDto> CreateAsync(ChecklistStepTemplateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, ChecklistStepTemplateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
