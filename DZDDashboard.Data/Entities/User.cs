namespace DZDDashboard.Data.Entities;

// User entity split across partial files by concern:
//   User.cs                — core identity + employment
//   User.Career.cs         — org structure + career assignment
//   User.PersonalInfo.cs   — PII (citizenship, address, family)
//   User.Benefits.cs       — pension, insurance, benefits
//   User.Navigation.cs     — navigation collections

public partial class User : AuditableEntity
{
    // ── Core identity ────────────────────────────────────────────────────────
    public int     Id              { get; set; }
    public string? EntraObjectId   { get; set; }
    public string? Email           { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? FirstName       { get; set; }
    public string? LastName        { get; set; }
    public bool    IsActive        { get; set; } = true;

    // ── Employment ───────────────────────────────────────────────────────────
    public string?   RegistrationNumber  { get; set; }
    public DateTime? UserStartDate       { get; set; }
    public DateTime? PositionStartDate   { get; set; }
    public DateTime? PositionUpdateDate  { get; set; }
    public string?   ContractType        { get; set; }
    public DateTime? ContractEndDate     { get; set; }
    public string?   WorkModel           { get; set; }
    public string?   UnitName            { get; set; }
    public string?   ApprovalProcessUnit { get; set; }
    public string?   EmployeeGroup       { get; set; }
    public string?   CvFilePath          { get; set; }
}
