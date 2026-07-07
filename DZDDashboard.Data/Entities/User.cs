namespace DZDDashboard.Data.Entities;


public partial class User : EntityWithHistory
{
    public int     Id              { get; set; }
    public string? EntraObjectId   { get; set; }
    public string? Email           { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? FirstName       { get; set; }
    public string? LastName        { get; set; }
    public bool    IsActive        { get; set; } = true;

    public string  Slug            { get; set; } = string.Empty;

    public int?    AvatarColorIndex { get; set; }

    public string?   RegistrationNumber  { get; set; }
    public DateTime? UserStartDate       { get; set; }
    public DateTime? PositionStartDate   { get; set; }
    public DateTime? PositionUpdateDate  { get; set; }
    public string?   ContractType        { get; set; }
    public DateTime? ContractEndDate     { get; set; }
    public string?   WorkModel           { get; set; }
}
