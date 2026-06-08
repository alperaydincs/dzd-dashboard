namespace DZDDashboard.Data.Entities;

public class Bank : AuditableEntity    
{
    public int Id { get; set; }
    public string BankName { get; set; } = string.Empty;
    public List<Project>? Projects { get; set; }
    public List<Itsm>? Itsms { get; set; }
}