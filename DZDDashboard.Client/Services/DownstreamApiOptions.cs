namespace DZDDashboard.Client.Services;

/// <summary>Typed options for the downstream API token scope configuration.</summary>
public sealed class DownstreamApiOptions
{
    public const string SectionName = "DownstreamApi";

    /// <summary>Space-separated OAuth scopes required to call the API.</summary>
    public string Scopes { get; set; } = string.Empty;
}
