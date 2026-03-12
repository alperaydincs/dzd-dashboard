namespace DZDDashboard.Common.DTOs;

public class UpdateFamilyInfoDto
{
    public int UserId { get; set; }
    public string? MaritalStatus { get; set; }
    public string? SpouseFullName { get; set; }
    public List<ChildInfoDto> Children { get; set; } = new();
}
