using System.ComponentModel.DataAnnotations;

namespace DZDDashboard.Common.DTOs;

public class CreateOrganizationPositionDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}

