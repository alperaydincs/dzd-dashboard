namespace DZDDashboard.Data.Entities
{
    public class UserTraining : AuditableEntity
    {
        public int Id { get; set; }
        public int? TrainingId { get; set; }
        public Training? Training { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? Status { get; set; }
        public int? Evaluation { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;    }
}
