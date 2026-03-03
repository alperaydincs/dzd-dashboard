using System.ComponentModel.DataAnnotations;

namespace DZDDashboard.Common.DTOs.Organization;

public class UpdateOrganizationPositionDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}
