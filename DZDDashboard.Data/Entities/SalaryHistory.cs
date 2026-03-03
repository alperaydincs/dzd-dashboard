namespace DZDDashboard.Data.Entities
{
    public class SalaryHistory
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
