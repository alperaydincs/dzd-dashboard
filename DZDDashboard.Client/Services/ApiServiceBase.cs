using System.Net.Http.Json;

namespace DZDDashboard.Client.Services;

public abstract class ApiServiceBase
{
    protected readonly HttpClient ApiClient;

    protected ApiServiceBase(IHttpClientFactory httpClientFactory)
    {
        ApiClient = httpClientFactory.CreateClient("Api");
    }

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            return await ApiClient.GetFromJsonAsync<T>(endpoint);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GET {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    protected async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            var response = await ApiClient.PostAsJsonAsync(endpoint, data);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<T>() : default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    protected async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
    {
        try
        {
            return await ApiClient.PostAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
    {
        try
        {
            return await ApiClient.PutAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PUT {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        try
        {
            return await ApiClient.DeleteAsync(endpoint);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DELETE {endpoint} failed: {ex.Message}");
            throw;
        }
    }
}
