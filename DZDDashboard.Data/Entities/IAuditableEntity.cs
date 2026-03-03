namespace DZDDashboard.Data.Entities;

public interface IAuditableEntity
{
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}