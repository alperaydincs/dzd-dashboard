namespace DZDDashboard.Common.DTOs;

public record UpdateFamilyInfoDto
{
    public string?         MaritalStatus  { get; init; }
    public string?         SpouseFullName { get; init; }
    public List<ChildInfoDto> Children   { get; init; } = [];
}
