namespace DZDDashboard.Data.Entities;

public partial class User
{
    public string?   AutoEnrollmentPensionStatus              { get; set; }
    public DateTime? EmployerPensionStartDate                  { get; set; }
    public bool      HasEmployerPension                        { get; set; }
    public decimal?  EmployerPensionEmployeeContribution       { get; set; }
    public decimal?  EmployerPensionEmployerContribution       { get; set; }

    public bool     HasPrivateHealthInsurance                  { get; set; }
    public decimal? PrivateHealthInsuranceEmployeeCost         { get; set; }
    public decimal? PrivateHealthInsuranceDependentCost        { get; set; }

    public decimal? MealBenefitAmount { get; set; }
}
