namespace DZDDashboard.Data.Entities
{
    public class GradeHistory
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
