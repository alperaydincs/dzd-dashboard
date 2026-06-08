namespace DZDDashboard.Common.DTOs;

/// <summary>
/// Immutable value object representing a duration in months or years.
/// Used as Blazor UI state in form components (e.g. GradeLevelDialog).
/// Not sent over the API — <see cref="RoleDurationDto"/> is the API transfer type.
/// </summary>
public sealed record DurationRange(int? Months = null, int? Years = null)
{
    public static readonly DurationRange Empty = new();
}
