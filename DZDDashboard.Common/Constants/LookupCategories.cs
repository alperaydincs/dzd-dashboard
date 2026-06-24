namespace DZDDashboard.Common.Constants;

public static class LookupCategories
{
    public const string AdditionalPaymentType = "AdditionalPaymentType";
    public const string DeductionType         = "DeductionType";
    public const string ContractType          = "ContractType";
    public const string WorkModel             = "WorkModel";
    public const string EducationLevel        = "EducationLevel";
    public const string DependentType         = "DependentType";

    public static readonly IReadOnlyList<string> All =
        [AdditionalPaymentType, DeductionType, ContractType, WorkModel, EducationLevel, DependentType];

    public static IReadOnlyList<string> DefaultsFor(string category) => category switch
    {
        AdditionalPaymentType => AdditionalPaymentTypes.All,
        DeductionType         => DeductionTypes.All,
        ContractType          => ContractTypes.All,
        WorkModel             => WorkModels.All,
        EducationLevel        => EducationLevels.All,
        DependentType         => DependentTypes.All,
        _                     => []
    };
}
