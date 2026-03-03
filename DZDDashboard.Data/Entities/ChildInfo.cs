namespace DZDDashboard.Data.Entities
{
    public class ChildInfo
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
