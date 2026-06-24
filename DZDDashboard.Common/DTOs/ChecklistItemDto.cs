namespace DZDDashboard.Common.DTOs;

public record ChecklistItemDependentInputDto
{
    public int     Order         { get; set; }
    public string  DependentType { get; set; } = string.Empty;
    public string? DependentName { get; set; }
    public decimal Amount        { get; set; }
}

public record ChecklistItemDto
{
    public int    Id          { get; set; }
    public string StepKey     { get; set; } = string.Empty;
    public string Title       { get; set; } = string.Empty;
    public int    Sequence    { get; set; }
    public bool   IsRequired  { get; set; }
    public bool   IsGate      { get; set; }
    public bool   RequiresDocument { get; set; }
    public string BenefitKind { get; set; } = string.Empty;
    public string Status      { get; set; } = string.Empty;
    public string? Note       { get; set; }

    public DateTime? CompletedAt    { get; set; }
    public string?   CompletedByName { get; set; }

    public bool    HasDocument      { get; set; }
    public string? DocumentFileName { get; set; }

    public string?  ProviderName   { get; set; }
    public string?  Currency       { get; set; }
    public decimal? EmployeeAmount { get; set; }
    public decimal? EmployerAmount { get; set; }

    public List<ChecklistItemDependentInputDto> Dependents { get; set; } = [];

    public bool    IsActionable { get; set; }
    public string? BlockedReason { get; set; }
}

public record CompleteChecklistItemDto
{
    public string? Note { get; set; }

    public string?  ProviderName   { get; set; }
    public string?  Currency       { get; set; }
    public decimal? EmployeeAmount { get; set; }
    public decimal? EmployerAmount { get; set; }

    public List<ChecklistItemDependentInputDto> Dependents { get; set; } = [];
}

public record UpdateChecklistNoteDto
{
    public string? Note { get; set; }
}
