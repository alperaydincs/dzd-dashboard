using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;

namespace DZDDashboard.Client.Services;

public interface IProcessTemplateClientService
{
    Task<List<ProcessTemplateDto>?> GetAsync(string kind);
    Task<ProcessTemplateDto?> CreateAsync(ProcessTemplateDto dto);
    Task<HttpResponseMessage> UpdateAsync(int id, ProcessTemplateDto dto);
    Task<HttpResponseMessage> DeleteAsync(int id);
}

public class ProcessTemplateClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), IProcessTemplateClientService
{
    public async Task<List<ProcessTemplateDto>?> GetAsync(string kind)
        => await GetAsync<List<ProcessTemplateDto>>(ApiRoutes.ProcessTemplates.List(kind));

    public async Task<ProcessTemplateDto?> CreateAsync(ProcessTemplateDto dto)
        => await PostAsync<ProcessTemplateDto>(ApiRoutes.ProcessTemplates.Base, dto);

    public async Task<HttpResponseMessage> UpdateAsync(int id, ProcessTemplateDto dto)
        => await PutAsync(ApiRoutes.ProcessTemplates.Item(id), dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await DeleteAsync(ApiRoutes.ProcessTemplates.Item(id));
}

public interface IChecklistTemplateClientService
{
    Task<List<ChecklistItemTemplateDto>?> GetAsync(int processTemplateId);
    Task<ChecklistItemTemplateDto?> CreateAsync(ChecklistItemTemplateDto dto);
    Task<HttpResponseMessage> UpdateAsync(int id, ChecklistItemTemplateDto dto);
    Task<HttpResponseMessage> DeleteAsync(int id);
}

public class ChecklistTemplateClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), IChecklistTemplateClientService
{
    public async Task<List<ChecklistItemTemplateDto>?> GetAsync(int processTemplateId)
        => await GetAsync<List<ChecklistItemTemplateDto>>(ApiRoutes.ChecklistTemplates.List(processTemplateId));

    public async Task<ChecklistItemTemplateDto?> CreateAsync(ChecklistItemTemplateDto dto)
        => await PostAsync<ChecklistItemTemplateDto>(ApiRoutes.ChecklistTemplates.Base, dto);

    public async Task<HttpResponseMessage> UpdateAsync(int id, ChecklistItemTemplateDto dto)
        => await PutAsync(ApiRoutes.ChecklistTemplates.Item(id), dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await DeleteAsync(ApiRoutes.ChecklistTemplates.Item(id));
}

public interface IDocumentTemplateClientService
{
    Task<List<DocumentTemplateDto>?> GetAsync(int processTemplateId);
    Task<DocumentTemplateDto?> CreateAsync(DocumentTemplateDto dto);
    Task<HttpResponseMessage> UpdateAsync(int id, DocumentTemplateDto dto);
    Task<HttpResponseMessage> DeleteAsync(int id);
}

public class DocumentTemplateClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), IDocumentTemplateClientService
{
    public async Task<List<DocumentTemplateDto>?> GetAsync(int processTemplateId)
        => await GetAsync<List<DocumentTemplateDto>>(ApiRoutes.DocumentTemplates.List(processTemplateId));

    public async Task<DocumentTemplateDto?> CreateAsync(DocumentTemplateDto dto)
        => await PostAsync<DocumentTemplateDto>(ApiRoutes.DocumentTemplates.Base, dto);

    public async Task<HttpResponseMessage> UpdateAsync(int id, DocumentTemplateDto dto)
        => await PutAsync(ApiRoutes.DocumentTemplates.Item(id), dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await DeleteAsync(ApiRoutes.DocumentTemplates.Item(id));
}
