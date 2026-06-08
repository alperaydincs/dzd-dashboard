namespace DZDDashboard.Common.Constants;

/// <summary>Application role names and policy names — single source of truth.</summary>
public static class Roles
{
    // ── Role names ────────────────────────────────────────────────────────────
    public const string Admin     = "Admin";
    public const string Hr        = "HR";

    // Future roles — add here as they are created in Azure Entra ID:
    // public const string HrManager = "HRManager";
    // public const string Finance   = "Finance";

    // ── Convenience composites for [Authorize(Roles = ...)] ───────────────────
    /// <summary>Comma-separated value for <c>[Authorize(Roles = Roles.AdminOrHr)]</c>.</summary>
    public const string AdminOrHr = $"{Admin},{Hr}";

    // ── Named policies (registered in ServiceCollectionExtensions) ────────────
    /// <summary>Policy: authenticated Admin or HR.</summary>
    public const string AdminOrHrPolicy = "AdminOrHr";

    /// <summary>
    /// Policy: roles allowed to read sensitive employee PII
    /// (CitizenshipNumber, DateOfBirth, PersonalContact, DisabilityStatus, etc.).
    /// Add new roles here when they are provisioned — policy is registered in one place.
    /// </summary>
    public const string SensitiveDataPolicy = "SensitiveEmployeeData";
}
