namespace DZDDashboard.Common.DTOs;

public class OrganizationPositionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public List<OrganizationPositionDto> Children { get; set; } = new();
    public int UserCount { get; set; }
    public int? UserId { get; set; }
    public UserDto? User { get; set; }
}

