using System.Text.Json.Serialization;

namespace DZDDashboard.Data.Entities;

public abstract class AuditableEntity
{
    public DateTime  CreatedAt    { get; set; }
    public DateTime? ModifiedAt   { get; set; }
    public int?      ModifiedById { get; set; }
    [JsonIgnore]
    public User?     ModifiedBy   { get; set; }
}
