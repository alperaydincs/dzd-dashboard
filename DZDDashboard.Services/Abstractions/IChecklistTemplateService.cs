using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IProcessTemplateService
{
    Task<List<ProcessTemplateDto>> GetAsync(string kind, CancellationToken cancellationToken = default);
    Task<ProcessTemplateDto> GetOneAsync(int id, CancellationToken cancellationToken = default);
    Task<ProcessTemplateDto> CreateAsync(ProcessTemplateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, ProcessTemplateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IChecklistTemplateService
{
    Task<List<ChecklistItemTemplateDto>> GetAsync(int processTemplateId, CancellationToken cancellationToken = default);
    Task<ChecklistItemTemplateDto> CreateAsync(ChecklistItemTemplateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, ChecklistItemTemplateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IDocumentTemplateService
{
    Task<List<DocumentTemplateDto>> GetAsync(int processTemplateId, CancellationToken cancellationToken = default);
    Task<DocumentTemplateDto> CreateAsync(DocumentTemplateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, DocumentTemplateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
