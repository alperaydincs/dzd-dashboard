namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by UpdateContactInfoDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
// Properties use `set` (not `init`) because this DTO is used as a Blazor form model
// with two-way @bind-Value bindings in ContactInfoDialog.razor.
public record UpdateContactInfoDto
{
    public string? WorkPhoneNumber     { get; set; }
    public string? PersonalEmail       { get; set; }
    public string? PersonalPhoneNumber { get; set; }
}
