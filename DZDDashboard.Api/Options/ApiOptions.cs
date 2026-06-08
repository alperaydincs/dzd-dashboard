namespace DZDDashboard.Api.Options;

public class ApiOptions
{
    public string[] AllowedCorsOrigins { get; set; } = [];
    public ApiRateLimitOptions RateLimit { get; set; } = new();
}

public class ApiRateLimitOptions
{
    public int GeneralPermitLimit   { get; set; } = 100;
    public int GeneralWindowMinutes { get; set; } = 1;
    public int UploadPermitLimit    { get; set; } = 10;
    public int UploadWindowMinutes  { get; set; } = 1;
}
