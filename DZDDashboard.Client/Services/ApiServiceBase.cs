using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.Identity.Web;
using System.Net;

namespace DZDDashboard.Client.Services;

public abstract class ApiServiceBase
{
    protected readonly HttpClient ApiClient;
    protected readonly NavigationManager _navigationManager;

    protected ApiServiceBase(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    {
        ApiClient = httpClientFactory.CreateClient("Api");
        _navigationManager = navigationManager;
    }

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            return await ApiClient.GetFromJsonAsync<T>(endpoint);
        }
        catch (Exception ex)
        {
            if (IsChallengeException(ex))
            {
                HandleChallengeException();
                return default;
            }
            Console.WriteLine($"GET {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    protected async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            var response = await ApiClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            if (IsChallengeException(ex))
            {
                HandleChallengeException();
                return default;
            }
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
            if (IsChallengeException(ex))
            {
                HandleChallengeException();
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
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
            if (IsChallengeException(ex))
            {
                HandleChallengeException();
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
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
            if (IsChallengeException(ex))
            {
                HandleChallengeException();
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            Console.WriteLine($"DELETE {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    protected async Task<HttpResponseMessage> PostMultipartAsync(string endpoint, MultipartFormDataContent content)
    {
        try
        {
            return await ApiClient.PostAsync(endpoint, content);
        }
        catch (Exception ex)
        {
            if (IsChallengeException(ex))
            {
                HandleChallengeException();
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            Console.WriteLine($"POST (multipart) {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    private bool IsChallengeException(Exception ex)
    {
        if (ex == null) return false;
        if (ex is System.Net.Http.HttpRequestException httpEx)
        {

            if (httpEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return true;

            if (!string.IsNullOrEmpty(httpEx.Message) && httpEx.Message.Contains("401"))
                return true;
        }

        if (ex is MicrosoftIdentityWebChallengeUserException ||
            (!string.IsNullOrEmpty(ex.Message) && (
                ex.Message.Contains("IDW10502") ||
                ex.Message.Contains("MsalUiRequiredException") ||
                ex.Message.Contains("interaction_required") ||
                ex.Message.Contains("consent_required") ||
                ex.Message.Contains("login_required")
            )))
        {
            return true;
        }

        return ex.InnerException != null && IsChallengeException(ex.InnerException);
    }

    private void HandleChallengeException()
    {
        var redirectUrl = "/auth/redirect";
        var currentUri = _navigationManager.Uri;

        if (!currentUri.Contains(redirectUrl, StringComparison.OrdinalIgnoreCase))
        {
            var returnUrl = Uri.EscapeDataString(currentUri);
            _navigationManager.NavigateTo($"{redirectUrl}?returnUrl={returnUrl}", forceLoad: false, replace: true);
        }
    }
}
