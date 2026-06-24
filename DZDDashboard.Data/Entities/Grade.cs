namespace DZDDashboard.Data.Entities;

public class Grade
{
    public int     Id         { get; set; }
    public string  Level      { get; set; } = string.Empty;
    public decimal MinSalary  { get; set; }
    public decimal MaxSalary  { get; set; }
    public string  Currency   { get; set; } = "TRY";
    public int?    NextStepId { get; set; }
    public Grade?  NextStep   { get; set; }
}
