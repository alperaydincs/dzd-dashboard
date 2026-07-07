namespace DZDDashboard.Data.Entities;

public class StoredFile : EntityWithHistory
{
    public int Id { get; set; }

    public byte[] Content { get; set; } = [];
    public string? ContentType { get; set; }
    public long SizeBytes { get; set; }
}
