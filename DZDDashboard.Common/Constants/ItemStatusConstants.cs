namespace DZDDashboard.Common.Constants;

public static class UserLifecycleStatuses
{
    public const string Candidate   = "Candidate";
    public const string Onboarding  = "Onboarding";
    public const string Active      = "Active";
    public const string Offboarding = "Offboarding";
    public const string Exited      = "Exited";

    public static readonly IReadOnlyList<string> All =
        [Candidate, Onboarding, Active, Offboarding, Exited];
}

public static class PositionChangeTypes
{
    public const string TitleAndGradeUpgrade = "Title & Grade Upgrade";
    public const string GradeUpgrade         = "Grade Upgrade";
    public const string TitleChange          = "Title Change";

    public static readonly IReadOnlyList<string> All = [TitleAndGradeUpgrade, GradeUpgrade, TitleChange];
}
