using System.Text.Json.Serialization;

namespace DZDDashboard.Data.Entities;

/// <summary>
/// Abstract base class that provides audit fields for all auditable entities.
/// Eliminates 4-property duplication across 36+ entity classes (DRY).
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>UTC timestamp set automatically on first insert by <c>AppDbContext.ApplyAuditInfo</c>. Never null after persistence.</summary>
    public DateTime  CreatedAt    { get; set; }
    /// <summary>UTC timestamp updated on every modification.</summary>
    public DateTime? ModifiedAt   { get; set; }
    public int?      ModifiedById { get; set; }

    /// <summary>
    /// Navigation to the last modifier. Never serialised — only used for auditing queries.
    /// Excluded from all AutoMapper mappings via explicit Ignore() in the mapping profiles.
    /// </summary>
    [JsonIgnore]
    public User?     ModifiedBy   { get; set; }
}
