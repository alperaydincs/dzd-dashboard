using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;

namespace DZDDashboard.Client.Services;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly string[] _scopes;
    private readonly ILogger<AuthTokenHandler> _logger;

    public AuthTokenHandler(
        ITokenAcquisition tokenAcquisition,
        IConfiguration configuration,
        ILogger<AuthTokenHandler> logger)
    {
        _tokenAcquisition = tokenAcquisition;
        _logger = logger;
        _scopes = (configuration["DownstreamApi:Scopes"] ?? string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (_scopes.Length == 0)
        {
            _logger.LogError("DownstreamApi:Scopes configuration is required for API token acquisition.");
            throw new InvalidOperationException("DownstreamApi:Scopes configuration is required for API token acquisition.");
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
            
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            else
            {
                _logger.LogWarning("Token acquisition returned empty token");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to acquire token");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
