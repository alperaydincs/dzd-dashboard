namespace DZDDashboard.Data.Entities;

public partial class User
{
    // ── Contact ──────────────────────────────────────────────────────────────
    public string? PhoneNumber          { get; set; }
    public string? PersonalEmail        { get; set; }
    public string? PersonalPhoneNumber  { get; set; }

    // ── Citizenship / identity (PII — access via SensitiveDataPolicy) ────────
    public DateTime? DateOfBirth      { get; set; }
    public string?   Gender           { get; set; }
    public string?   Nationality      { get; set; }
    public string?   CitizenshipNumber { get; set; }
    public bool      DisabilityStatus  { get; set; } = false;
    public string?   DisabilityDegree  { get; set; }

    // ── Family (PII) ─────────────────────────────────────────────────────────
    public string?           MaritalStatus  { get; set; }
    public string?           SpouseFullName { get; set; }
    public List<ChildInfo>?  Children       { get; set; }

    // ── Address (PII) ────────────────────────────────────────────────────────
    public string? LegalAddress   { get; set; }
    public string? CurrentAddress { get; set; }
    public string? City           { get; set; }
    public string? Country        { get; set; }

    // ── Banking (PII — TODO: expose via dedicated endpoint with Finance role) ─
    public string? BankName { get; set; }
    public string? Iban     { get; set; }
}
