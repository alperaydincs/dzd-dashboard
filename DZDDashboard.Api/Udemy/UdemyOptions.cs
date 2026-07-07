namespace DZDDashboard.Api.Udemy;

public class UdemyOptions
{
    public const string SectionName = "Udemy";
    public bool Enabled { get; set; }
    public string BaseUrl { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public int SyncIntervalHours { get; set; } = 12;
    public int PageSize { get; set; } = 100;

    public bool IsConfigured =>
        Enabled
        && !string.IsNullOrWhiteSpace(BaseUrl)
        && !string.IsNullOrWhiteSpace(AccountId)
        && !string.IsNullOrWhiteSpace(ClientId)
        && !string.IsNullOrWhiteSpace(ClientSecret);
}
