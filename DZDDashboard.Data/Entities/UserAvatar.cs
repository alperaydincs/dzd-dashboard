namespace DZDDashboard.Data.Entities
{
    public class UserAvatar : IAuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ContentBase64 { get; set; } = string.Empty;
        public string? ContentType { get; set; }
        public User? User { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedById { get; set; }
        public User? ModifiedBy { get; set; }
    }
}