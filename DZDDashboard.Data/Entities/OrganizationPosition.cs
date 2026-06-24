namespace DZDDashboard.Data.Entities;

public class OrganizationPosition : AuditableEntity
{
    public int    Id       { get; set; }
    public string Name     { get; set; } = string.Empty;
    public int?   ParentId { get; set; }
    public OrganizationPosition?              Parent   { get; set; }
    public ICollection<OrganizationPosition> Children { get; set; } = new List<OrganizationPosition>();
    public ICollection<User>                 Users    { get; set; } = new List<User>();
}
