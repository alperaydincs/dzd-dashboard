namespace DZDDashboard.Api.Udemy;

public class UdemyOptions
{
    public const string SectionName = "Udemy";

    /// <summary>Master switch; when false the sync background service does nothing.</summary>
    public bool Enabled { get; set; }

    /// <summary>Base URL, e.g. https://dzdtech.udemy.com/api-2.0/ (trailing slash required).</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>Numeric organization/account id used in the report endpoints.</summary>
    public string AccountId { get; set; } = string.Empty;

    /// <summary>Udemy API client id (HTTP Basic username). Store via secrets, not appsettings.</summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>Udemy API client secret (HTTP Basic password). Store via secrets, not appsettings.</summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>How often the sync runs.</summary>
    public int SyncIntervalHours { get; set; } = 12;

    /// <summary>Page size for the paginated activity endpoint.</summary>
    public int PageSize { get; set; } = 100;

    public bool IsConfigured =>
        Enabled
        && !string.IsNullOrWhiteSpace(BaseUrl)
        && !string.IsNullOrWhiteSpace(AccountId)
        && !string.IsNullOrWhiteSpace(ClientId)
        && !string.IsNullOrWhiteSpace(ClientSecret);
}
