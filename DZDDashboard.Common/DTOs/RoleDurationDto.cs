namespace DZDDashboard.Common.DTOs;

/// <summary>
/// API transfer type for the <c>RoleDuration</c> owned value object on <c>CareerMapRule</c>.
/// Mutable (<c>set</c>) so FluentValidation and model binding can assign properties.
/// Only one of <see cref="Months"/> or <see cref="Years"/> should be set at a time;
/// that rule is enforced by <c>RoleDurationDtoValidator</c>.
/// Not used for UI state — see <see cref="DurationRange"/> for that.
/// </summary>
public record RoleDurationDto
{
    public int? Months { get; set; }
    public int? Years  { get; set; }
}
