using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;

namespace DZDDashboard.Client.Services;

public interface IChecklistTemplateClientService
{
    Task<List<ChecklistStepTemplateDto>?> GetAsync(string processType);
    Task<ChecklistStepTemplateDto?> CreateAsync(ChecklistStepTemplateDto dto);
    Task<HttpResponseMessage> UpdateAsync(int id, ChecklistStepTemplateDto dto);
    Task<HttpResponseMessage> DeleteAsync(int id);
}

public class ChecklistTemplateClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), IChecklistTemplateClientService
{
    public async Task<List<ChecklistStepTemplateDto>?> GetAsync(string processType)
        => await GetAsync<List<ChecklistStepTemplateDto>>(ApiRoutes.ChecklistTemplates.List(processType));

    public async Task<ChecklistStepTemplateDto?> CreateAsync(ChecklistStepTemplateDto dto)
        => await PostAsync<ChecklistStepTemplateDto>(ApiRoutes.ChecklistTemplates.Base, dto);

    public async Task<HttpResponseMessage> UpdateAsync(int id, ChecklistStepTemplateDto dto)
        => await PutAsync(ApiRoutes.ChecklistTemplates.Item(id), dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await DeleteAsync(ApiRoutes.ChecklistTemplates.Item(id));
}
