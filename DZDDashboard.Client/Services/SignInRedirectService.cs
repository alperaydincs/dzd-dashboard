using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace DZDDashboard.Client.Services;

public class SignInRedirectService
{
    private readonly NavigationManager _navigationManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public event Action? Changed;

    public bool IsRedirecting { get; private set; }

    public SignInRedirectService(NavigationManager navigationManager, IHttpContextAccessor httpContextAccessor)
    {
        _navigationManager = navigationManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RedirectToSignInAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var returnUrl = BuildReturnUrl(httpContext);
        var signInUrl = $"/MicrosoftIdentity/Account/SignIn?returnUrl={Uri.EscapeDataString(returnUrl)}";

        IsRedirecting = true;
        Changed?.Invoke();

        await Task.Delay(150);

        if (httpContext is not null && !httpContext.Response.HasStarted)
        {
            httpContext.Response.Redirect(signInUrl);
            return;
        }
        else
        {
            try
            {
                _navigationManager.NavigateTo(signInUrl, forceLoad: true, replace: true);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("has not been initialized", StringComparison.OrdinalIgnoreCase))
            {

            }
        }

    }

    private string BuildReturnUrl(HttpContext? httpContext)
    {
        if (httpContext is not null)
        {
            var path = httpContext.Request.Path.HasValue ? httpContext.Request.Path.Value : "/";
            var queryString = httpContext.Request.QueryString.HasValue ? httpContext.Request.QueryString.Value : string.Empty;
            return string.IsNullOrWhiteSpace(path) ? "/" : $"{path}{queryString}";
        }

        var relativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
        return string.IsNullOrWhiteSpace(relativePath)
            ? "/"
            : $"/{relativePath}";
    }
}