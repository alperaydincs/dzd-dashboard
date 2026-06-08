namespace DZDDashboard.Common.Constants;

/// <summary>
/// JWT claim type constants shared by the API and the Data layer.
/// Centralised here (Common) so both layers reference the same literals
/// without creating an upward dependency from Data → Api.
/// </summary>
public static class DzdClaimTypes
{
    public const string DatabaseUserId        = "database_user_id";
    public const string ObjectIdentifier      = "http://schemas.microsoft.com/identity/claims/objectidentifier";
    public const string ObjectIdentifierShort = "oid";
    /// <summary>Standard OIDC subject claim — fallback when OID-specific claims are absent.</summary>
    public const string Subject               = "sub";
    /// <summary>The Azure AD role claim type used in JWT tokens.</summary>
    public const string RoleClaimType         = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
}
