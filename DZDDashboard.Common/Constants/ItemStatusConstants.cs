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

public static class ProcessStatuses
{
    public const string InProgress = "InProgress";
    public const string Completed  = "Completed";
    public const string Cancelled  = "Cancelled";

    public static readonly IReadOnlyList<string> All = [InProgress, Completed, Cancelled];
}

public static class ChecklistItemStatuses
{
    public const string Pending   = "Pending";
    public const string Completed = "Completed";

    public static readonly IReadOnlyList<string> All = [Pending, Completed];
}

public static class DocumentReviewStatuses
{
    public const string Pending         = "Pending";
    public const string Uploaded        = "Uploaded";
    public const string Approved        = "Approved";
    public const string NeedsCorrection = "NeedsCorrection";

    public static readonly IReadOnlyList<string> All = [Pending, Uploaded, Approved, NeedsCorrection];
}

public static class PositionChangeTypes
{
    public const string TitleAndGradeUpgrade = "Title & Grade Upgrade";
    public const string GradeUpgrade         = "Grade Upgrade";
    public const string TitleChange          = "Title Change";

    public static readonly IReadOnlyList<string> All = [TitleAndGradeUpgrade, GradeUpgrade, TitleChange];
}

public static class ProcessTemplateKinds
{
    public const string Onboarding  = "Onboarding";
    public const string Offboarding = "Offboarding";

    public static readonly IReadOnlyList<string> All = [Onboarding, Offboarding];
}

public static class OnboardingOffboardingAuditActions
{
    public const string Started                   = "Started";
    public const string DocumentAdded              = "DocumentAdded";
    public const string DocumentUploaded           = "DocumentUploaded";
    public const string DocumentApproved           = "DocumentApproved";
    public const string DocumentCorrectionRequested = "DocumentCorrectionRequested";
    public const string DocumentDeleted            = "DocumentDeleted";
    public const string ChecklistCompleted         = "ChecklistCompleted";
    public const string ChecklistReopened          = "ChecklistReopened";
    public const string ProcessCompleted           = "ProcessCompleted";
    public const string ProcessCancelled           = "ProcessCancelled";
}
