namespace DZDDashboard.Data.Entities;

public class CareerPathRuleJob
{
    public int CareerPathRuleId { get; set; }
    public CareerPathRule? CareerPathRule { get; set; }
    public int JobId { get; set; }
    public Job? Job { get; set; }
}
