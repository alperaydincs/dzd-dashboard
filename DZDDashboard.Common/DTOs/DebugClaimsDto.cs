namespace DZDDashboard.Common.DTOs;

public class DebugClaimsDto
{
    public bool Authenticated { get; set; }
    public string? IdentityName { get; set; }
    public List<ClaimDto> Claims { get; set; } = new();
    public List<string> Roles { get; set; } = new();
    public int RolesCount { get; set; }
}

public class ClaimDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

