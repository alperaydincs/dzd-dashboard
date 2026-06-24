namespace DZDDashboard.Data.Entities;

public partial class User
{
    public string? PhoneNumber          { get; set; }
    public string? PersonalEmail        { get; set; }
    public string? PersonalPhoneNumber  { get; set; }

    public DateTime? DateOfBirth      { get; set; }
    public string?   Gender           { get; set; }
    public string?   Nationality      { get; set; }
    public string?   CitizenshipNumber { get; set; }
    public bool      DisabilityStatus  { get; set; } = false;
    public string?   DisabilityDegree  { get; set; }

    public string?           MaritalStatus  { get; set; }
    public string?           SpouseFullName { get; set; }
    public List<ChildInfo>?  Children       { get; set; }

    public string? LegalAddress        { get; set; }
    public string? LegalAddressCity    { get; set; }
    public string? LegalAddressCountry { get; set; }
    public string? CurrentAddress      { get; set; }
    public string? City                { get; set; }    public string? Country             { get; set; }
    public string? BankName { get; set; }
    public string? Iban     { get; set; }
}
