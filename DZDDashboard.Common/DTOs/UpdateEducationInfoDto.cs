namespace DZDDashboard.Common.DTOs;

public record UpdateEducationInfoDto
{
    public List<EducationHistoryDto> EducationHistories { get; init; } = [];
}
