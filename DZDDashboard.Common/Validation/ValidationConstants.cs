namespace DZDDashboard.Common.Validation;

public static class ValidationConstants
{
    public const int MaxNameLength = 100;
    public const int MaxFullNameLength = 200;
    public const int MaxEmailLength = 256;
    public const int MaxPhoneLength = 20;
    public const int MaxAddressLength = 500;
    public const int MaxShortNameLength = 50;
    public const int MaxNumericIdentifierLength = 20;
    public const int MaxStandardLength   = 200;
    /// <summary>DB column width for Department, Team, Job, PayrollLocation, UserGroup names.</summary>
    public const int MaxEntityNameLength = 150;
    /// <summary>DB column width for EducationHistory.Institution.</summary>
    public const int MaxInstitutionLength = 300;
    /// <summary>DB column width for EducationHistory.Level.</summary>
    public const int MaxEducationLevelLength = 100;

    // Career / grade
    public const int MinGrade = 1;
    public const int MaxGrade = 100;
    public const int MaxRoleTimeYears  = 100;
    public const int MaxRoleTimeMonths = MaxRoleTimeYears * 12; // derived — change only MaxRoleTimeYears

    // Misc
    public const int MaxCurrencyCodeLength  = 10;
    public const int MaxGradeLevelLength    = 50;
    public const int MaxPositionNameLength  = 100;

    /// <summary>DB column width for free-text description fields (Company.Description, WorkType.Description).</summary>
    public const int MaxDescriptionLength   = 500;

    // Project / ITSM / document domain
    /// <summary>DB column width for file/document names (DefaultDocument, ProjectDocument, UserDocument).</summary>
    public const int MaxFileNameLength      = 255;
    /// <summary>DB column width for MIME content-type strings.</summary>
    public const int MaxContentTypeLength   = 100;
    /// <summary>DB column width for project/invoice/bid names.</summary>
    public const int MaxProjectNameLength   = 250;
    /// <summary>DB column width for free-text notes and info fields.</summary>
    public const int MaxNotesLength         = 1000;
    /// <summary>DB column width for short reference codes (Jira project/task numbers, T-shirt sizes).</summary>
    public const int MaxReferenceCodeLength = 50;
}