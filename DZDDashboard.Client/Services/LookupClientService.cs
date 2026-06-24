using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;

namespace DZDDashboard.Client.Services;

public interface ILookupClientService
{
    Task<List<string>> GetValuesAsync(string category);
    Task<List<LookupValueDto>?> GetAllAsync(string category);
    Task<LookupValueDto?> CreateAsync(LookupValueDto dto);
    Task<HttpResponseMessage> UpdateAsync(int id, LookupValueDto dto);
    Task<HttpResponseMessage> DeleteAsync(int id);
}

public class LookupClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), ILookupClientService
{
    public async Task<List<string>> GetValuesAsync(string category)
    {
        var list = await GetAsync<List<LookupValueDto>>(ApiRoutes.Lookups.List(category));
        return list is null ? [] : [.. list.Select(x => x.Value)];
    }

    public async Task<List<LookupValueDto>?> GetAllAsync(string category)
        => await GetAsync<List<LookupValueDto>>(ApiRoutes.Lookups.List(category));

    public async Task<LookupValueDto?> CreateAsync(LookupValueDto dto)
        => await PostAsync<LookupValueDto>(ApiRoutes.Lookups.Base, dto);

    public async Task<HttpResponseMessage> UpdateAsync(int id, LookupValueDto dto)
        => await PutAsync(ApiRoutes.Lookups.Item(id), dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await DeleteAsync(ApiRoutes.Lookups.Item(id));
}
