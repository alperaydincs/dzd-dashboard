
using Microsoft.Identity.Web;
using System.Net;

namespace DZDDashboard.Client.Services;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SignInRedirectService _signInRedirectService;

    private readonly string[] _scopes;


    public AuthTokenHandler(
        ITokenAcquisition tokenAcquisition,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        SignInRedirectService signInRedirectService)
    {
        _tokenAcquisition = tokenAcquisition;
        _httpContextAccessor = httpContextAccessor;
        _signInRedirectService = signInRedirectService;

        _scopes = (configuration["DownstreamApi:Scopes"] ?? string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (_scopes.Length == 0)
        {
            throw new InvalidOperationException("DownstreamApi:Scopes configuration is required for API token acquisition.");
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var user = httpContext.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        try
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes, user: user);

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await _signInRedirectService.RedirectToSignInAsync();

            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = request,
                ReasonPhrase = "Authentication challenge required"
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
