using System.ComponentModel.DataAnnotations;

namespace DZDDashboard.Data.Entities;

public class OrganizationPosition : IAuditableEntity
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public virtual OrganizationPosition? Parent { get; set; }
    public virtual ICollection<OrganizationPosition> Children { get; set; } = new List<OrganizationPosition>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
