using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;

namespace DZDDashboard.Client.Services;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly ITokenAcquisition    _tokenAcquisition;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string[]             _scopes;

    public AuthTokenHandler(
        ITokenAcquisition tokenAcquisition,
        IHttpContextAccessor httpContextAccessor,
        IOptions<DownstreamApiOptions> options)
    {
        _tokenAcquisition    = tokenAcquisition;
        _httpContextAccessor = httpContextAccessor;

        _scopes = options.Value.Scopes
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (_scopes.Length == 0)
            throw new InvalidOperationException(
                $"{DownstreamApiOptions.SectionName}:Scopes configuration is required for API token acquisition.");
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            return await base.SendAsync(request, cancellationToken);

        var user = httpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
            return await base.SendAsync(request, cancellationToken);

        try
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes, user: user);

            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = request,
                ReasonPhrase   = "Authentication challenge required"
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
