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
    public const int MaxEntityNameLength = 150;
    public const int MaxInstitutionLength = 300;
    public const int MaxEducationLevelLength = 100;

    public const int MinGrade = 1;
    public const int MaxGrade = 100;
    public const int MaxRoleTimeYears  = 100;
    public const int MaxRoleTimeMonths = MaxRoleTimeYears * 12;
    public const int MaxCurrencyCodeLength  = 10;
    public const int MaxGradeLevelLength    = 50;
    public const int MaxPositionNameLength  = 100;

    public const int MaxDescriptionLength   = 500;

    public const int MaxFileNameLength      = 255;
    public const int MaxContentTypeLength   = 100;
    public const int MaxProjectNameLength   = 250;
    public const int MaxNotesLength         = 1000;
    public const int MaxReferenceCodeLength = 50;

    public const int MaxProviderNameLength = 200;
    public const int MaxBenefitDependents  = 5;
    public const int MaxBenefitNameLength   = 150;
    public const int MaxPolicyNumberLength  = 50;
}