namespace DZDDashboard.Common.Constants;

public static class ContractTypes
{
    public const string FullTime   = "Full-time";
    public const string PartTime   = "Part-time";
    public const string Contract   = "Contract";
    public const string Temporary  = "Temporary";

    public static readonly IReadOnlyList<string> All = [FullTime, PartTime, Contract, Temporary];
}

public static class WorkModels
{
    public const string Remote  = "Remote";
    public const string Hybrid  = "Hybrid";
    public const string OnSite  = "On-site";

    public static readonly IReadOnlyList<string> All = [Remote, Hybrid, OnSite];
}

public static class GenderValues
{
    public const string Male   = "Male";
    public const string Female = "Female";

    public static readonly IReadOnlyList<string> All = [Male, Female];
}

public static class EducationStatuses
{
    public const string Completed  = "Completed";
    public const string InProgress = "In Progress";
    public const string Planned    = "Planned";

    public static readonly IReadOnlyList<string> All = [Completed, InProgress, Planned];
}

public static class MaritalStatuses
{
    public const string Single   = "Single";
    public const string Married  = "Married";
    public const string Divorced = "Divorced";
    public const string Widowed  = "Widowed";

    public static readonly IReadOnlyList<string> All = [Single, Married, Divorced, Widowed];
}

public static class EducationLevels
{
    public const string HighSchool      = "High School";
    public const string Associate       = "Associate";
    public const string Bachelors       = "Bachelor's Degree";
    public const string Masters         = "Master's Degree";
    public const string Phd             = "PhD";
    public const string Other           = "Other";

    public static readonly IReadOnlyList<string> All = [HighSchool, Associate, Bachelors, Masters, Phd, Other];
}
