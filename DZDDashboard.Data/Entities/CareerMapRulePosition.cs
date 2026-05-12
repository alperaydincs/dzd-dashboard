namespace DZDDashboard.Data.Entities;

public class CareerMapRulePosition
{
    public int CareerMapRuleId { get; set; }
    public CareerMapRule? CareerMapRule { get; set; }
    public int JobId { get; set; }
    public Job? Job { get; set; }
}
