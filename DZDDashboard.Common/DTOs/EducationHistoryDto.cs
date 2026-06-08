namespace DZDDashboard.Common.DTOs;

// FluentValidation (UpdateEducationInfoDtoValidator) is the single enforcement point —
// no [MaxLength] annotations here to avoid dual-validation and value mismatches.
public record EducationHistoryDto
{
    public int Id { get; set; }

    public string? Level          { get; set; }
    public string? Institution    { get; set; }
    public string? Program        { get; set; }
    public DateTime? GraduationDate { get; set; }
    public string? Status         { get; set; }
}
