using System.Net.Http.Json;
using Microsoft.Identity.Web;
using System.Net;

namespace DZDDashboard.Client.Services;

public abstract class ApiServiceBase
{
    protected readonly HttpClient ApiClient;
    private readonly SignInRedirectService _signInRedirectService;

    protected ApiServiceBase(IHttpClientFactory httpClientFactory, SignInRedirectService signInRedirectService)
    {
        ApiClient = httpClientFactory.CreateClient("Api");
        _signInRedirectService = signInRedirectService;
    }

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await ApiClient.GetAsync(endpoint);
            if (await HandleUnauthorizedResponseAsync(response))
            {
                return default;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await _signInRedirectService.RedirectToSignInAsync();
            return default;
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
            if (await HandleUnauthorizedResponseAsync(response))
            {
                return default;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await _signInRedirectService.RedirectToSignInAsync();
            return default;
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
            var response = await ApiClient.PostAsJsonAsync(endpoint, data);
            await HandleUnauthorizedResponseAsync(response);
            return response;
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await _signInRedirectService.RedirectToSignInAsync();
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
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
            var response = await ApiClient.PutAsJsonAsync(endpoint, data);
            await HandleUnauthorizedResponseAsync(response);
            return response;
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await _signInRedirectService.RedirectToSignInAsync();
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
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
            var response = await ApiClient.DeleteAsync(endpoint);
            await HandleUnauthorizedResponseAsync(response);
            return response;
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await _signInRedirectService.RedirectToSignInAsync();
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DELETE {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    protected async Task<HttpResponseMessage> PostMultipartAsync(string endpoint, MultipartFormDataContent content)
    {
        try
        {
            var response = await ApiClient.PostAsync(endpoint, content);
            await HandleUnauthorizedResponseAsync(response);
            return response;
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await _signInRedirectService.RedirectToSignInAsync();
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST (multipart) {endpoint} failed: {ex.Message}");
            throw;
        }
    }

    private async Task<bool> HandleUnauthorizedResponseAsync(HttpResponseMessage response)
    {
        if (response.StatusCode != HttpStatusCode.Unauthorized)
        {
            return false;
        }

        await _signInRedirectService.RedirectToSignInAsync();
        return true;
    }
}
