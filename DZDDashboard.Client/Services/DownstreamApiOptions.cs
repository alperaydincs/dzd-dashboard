namespace DZDDashboard.Client.Services;

public sealed class DownstreamApiOptions
{
    public const string SectionName = "DownstreamApi";

    public string Scopes { get; set; } = string.Empty;
}
