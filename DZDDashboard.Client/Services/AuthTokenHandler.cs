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
        
        _logger.LogInformation($"AuthTokenHandler initialized with scopes: {string.Join(", ", _scopes)}");
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Acquiring token for request to {request.RequestUri}");
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
            
            if (!string.IsNullOrEmpty(accessToken))
            {
                _logger.LogInformation("Token acquired successfully, adding to request");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            else
            {
                _logger.LogWarning("Token acquisition returned empty token");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to acquire token: {ex.Message}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
