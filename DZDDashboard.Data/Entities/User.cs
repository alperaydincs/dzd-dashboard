namespace DZDDashboard.Data.Entities;


public partial class User : AuditableEntity
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
    public int?      ContractTypeId      { get; set; }
    public ContractTypeEntity? ContractTypeRef { get; set; }
    public DateTime? ContractEndDate     { get; set; }
    public int?      WorkModelId         { get; set; }
    public WorkModelEntity? WorkModelRef { get; set; }
    public string?   ApprovalProcessUnit { get; set; }
    public string?   EmployeeGroup       { get; set; }
    public string?   CvFilePath          { get; set; }
}
