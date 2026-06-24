namespace DZDDashboard.Data.Entities;

public class StoredFile : AuditableEntity
{
    public int Id { get; set; }

    public byte[] Content { get; set; } = [];
    public string? ContentType { get; set; }
    public long SizeBytes { get; set; }
}
