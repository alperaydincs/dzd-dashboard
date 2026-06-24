using DZDDashboard.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text.Json;

namespace DZDDashboard.Client.Services;

public abstract class ApiServiceBase
{
    protected readonly HttpClient ApiClient;
    private readonly NavigationManager _navigationManager;

    protected ApiServiceBase(
        IHttpClientFactory httpClientFactory,
        NavigationManager navigationManager)
    {
        ApiClient          = httpClientFactory.CreateClient("Api");
        _navigationManager = navigationManager;
    }

    protected async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            return await ApiClient.GetFromJsonAsync<T>(endpoint, cancellationToken);
        }
        catch (Exception ex)
        {
            if (IsAuthChallenge(ex)) { HandleChallenge(); return default; }
            throw;
        }
    }

    protected async Task<T?> PostAsync<T>(string endpoint, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await ApiClient.PostAsJsonAsync(endpoint, data, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        }
        catch (Exception ex)
        {
            if (IsAuthChallenge(ex)) { HandleChallenge(); return default; }
            throw;
        }
    }

    protected async Task<HttpResponseMessage> PostAsync(string endpoint, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            return await ApiClient.PostAsJsonAsync(endpoint, data, cancellationToken);
        }
        catch (Exception ex)
        {
            if (IsAuthChallenge(ex)) { HandleChallenge(); return Unauthorized(); }
            throw;
        }
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data, CancellationToken cancellationToken = default)
    {
        try
        {
            return await ApiClient.PutAsJsonAsync(endpoint, data, cancellationToken);
        }
        catch (Exception ex)
        {
            if (IsAuthChallenge(ex)) { HandleChallenge(); return Unauthorized(); }
            throw;
        }
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            return await ApiClient.DeleteAsync(endpoint, cancellationToken);
        }
        catch (Exception ex)
        {
            if (IsAuthChallenge(ex)) { HandleChallenge(); return Unauthorized(); }
            throw;
        }
    }

    protected async Task<HttpResponseMessage> PostMultipartAsync(string endpoint, MultipartFormDataContent content, CancellationToken cancellationToken = default)
    {
        try
        {
            return await ApiClient.PostAsync(endpoint, content, cancellationToken);
        }
        catch (Exception ex)
        {
            if (IsAuthChallenge(ex)) { HandleChallenge(); return Unauthorized(); }
            throw;
        }
    }

    public static async Task<string?> TryReadProblemDetailAsync(HttpResponseMessage response)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content)) return null;

            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.ValueKind == JsonValueKind.Object
                && doc.RootElement.TryGetProperty("detail", out var detail))
            {
                var msg = detail.GetString();
                return string.IsNullOrWhiteSpace(msg) ? null : msg;
            }
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }


    private static bool IsAuthChallenge(Exception ex)
    {
        if (ex is HttpRequestException { StatusCode: HttpStatusCode.Unauthorized })
            return true;

        var current = ex;
        while (current != null)
        {
            if (current.GetType().FullName is
                "Microsoft.Identity.Web.MicrosoftIdentityWebChallengeUserException" or
                "Microsoft.Identity.Client.MsalUiRequiredException")
                return true;
            current = current.InnerException;
        }
        return false;
    }

    private void HandleChallenge()
    {
        var redirectUrl = RouteConstants.AuthRedirect;
        var currentUri = _navigationManager.Uri;
        if (!currentUri.Contains(redirectUrl, StringComparison.OrdinalIgnoreCase))
        {
            var returnUrl = Uri.EscapeDataString(currentUri);
            _navigationManager.NavigateTo($"{redirectUrl}?returnUrl={returnUrl}", forceLoad: false, replace: true);
        }
    }

    private static HttpResponseMessage Unauthorized()
        => new(HttpStatusCode.Unauthorized);
}
