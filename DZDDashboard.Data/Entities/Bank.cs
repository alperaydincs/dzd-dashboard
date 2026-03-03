namespace DZDDashboard.Data.Entities;

public class Bank : IAuditableEntity    
{
    public int Id { get; set; }
    public string? BankName { get; set; }
    public List<Project>? Projects { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}