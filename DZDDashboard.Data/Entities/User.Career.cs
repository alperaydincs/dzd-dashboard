namespace DZDDashboard.Data.Entities;

public partial class User
{
    public int?               CompanyId            { get; set; }
    public Company?           Company              { get; set; }
    public int?               DepartmentId         { get; set; }
    public Department?        Department           { get; set; }
    public int?               TeamId               { get; set; }
    public Team?              Team                 { get; set; }
    public int?               PayrollLocationId    { get; set; }
    public PayrollLocation?   PayrollLocation      { get; set; }
    public int?               OrganizationPositionId { get; set; }
    public OrganizationPosition? OrganizationPosition { get; set; }
    public User?              ReportsTo            { get; set; }
    public int?               ReportsToId          { get; set; }
    public ICollection<User>  Subordinates         { get; set; } = new List<User>();
    public int?        JobId        { get; set; }
    public Job?        Job          { get; set; }
    public int?        Grade        { get; set; }
    public int?        CareerPathId { get; set; }
    public CareerPath? CareerPath   { get; set; }

    public int?        AvatarId { get; set; }
    public StoredFile? Avatar   { get; set; }
}
