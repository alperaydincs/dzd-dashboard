namespace DZDDashboard.Data.Entities
{
    public class UserGroup : IAuditableEntity
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }
        public List<User>? User { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedById { get; set; }
        public User? ModifiedBy { get; set; }
    }
}
