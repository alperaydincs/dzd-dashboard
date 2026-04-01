namespace DZDDashboard.Common.DTOs;

public class UpdateEducationInfoDto
{
    public int UserId { get; set; }
    public List<EducationHistoryDto> EducationHistories { get; set; } = new();
}
