namespace DZDDashboard.Common.DTOs;

public record UpdateBasicInfoDto
{
    public string?   FirstName          { get; init; }
    public string?   LastName           { get; init; }
    public string?   RegistrationNumber { get; init; }
    public DateTime? UserStartDate      { get; init; }
    public DateTime? PositionStartDate  { get; init; }
    public DateTime? ContractEndDate    { get; init; }
    public string?   ContractType       { get; init; }
    public string?   WorkModel          { get; init; }
    public int?      PayrollLocationId  { get; init; }
}
