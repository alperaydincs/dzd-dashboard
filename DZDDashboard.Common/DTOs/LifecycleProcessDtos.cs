namespace DZDDashboard.Common.DTOs;

public record OnboardingListItemDto
{
    public int      Id             { get; set; }
    public int      UserId         { get; set; }
    public string?  EmployeeName   { get; set; }
    public string?  EmployeeSlug   { get; set; }
    public DateTime StartDate      { get; set; }
    public string   Status         { get; set; } = string.Empty;
    public int      CompletedCount { get; set; }
    public int      TotalCount     { get; set; }
}

public record OnboardingProcessDto
{
    public int      Id           { get; set; }
    public int      UserId       { get; set; }
    public string?  EmployeeName { get; set; }
    public string?  EmployeeSlug { get; set; }
    public DateTime StartDate    { get; set; }
    public int?     ManagerId    { get; set; }
    public string?  ManagerName  { get; set; }
    public string   Status       { get; set; } = string.Empty;
    public DateTime? CompletedAt { get; set; }

    public List<ChecklistItemDto> Items { get; set; } = [];
}

public record OffboardingListItemDto
{
    public int      Id             { get; set; }
    public int      UserId         { get; set; }
    public string?  EmployeeName   { get; set; }
    public string?  EmployeeSlug   { get; set; }
    public string   Type           { get; set; } = string.Empty;
    public DateTime ExitDate       { get; set; }
    public string   Status         { get; set; } = string.Empty;
    public int      CompletedCount { get; set; }
    public int      TotalCount     { get; set; }
}

public record OffboardingProcessDto
{
    public int      Id           { get; set; }
    public int      UserId       { get; set; }
    public string?  EmployeeName { get; set; }
    public string?  EmployeeSlug { get; set; }
    public string   Type         { get; set; } = string.Empty;
    public DateTime ExitDate     { get; set; }
    public string   Status       { get; set; } = string.Empty;
    public DateTime? CompletedAt { get; set; }

    public List<ChecklistItemDto> Items { get; set; } = [];
}

public record StartOnboardingDto
{
    public string  FirstName     { get; set; } = string.Empty;
    public string  LastName      { get; set; } = string.Empty;
    public string? Email         { get; set; }
    public string? PersonalEmail { get; set; }
    public string? PhoneNumber   { get; set; }
    public DateTime StartDate    { get; set; }
    public int?    ManagerId     { get; set; }
}

public record StartOffboardingDto
{
    public int      UserId   { get; set; }
    public string   Type     { get; set; } = string.Empty;
    public DateTime ExitDate { get; set; }
}
