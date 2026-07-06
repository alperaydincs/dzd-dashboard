namespace DZDDashboard.Common.Constants;
public static class DomainCategories
{
    public const string ContractType          = "contractType";
    public const string WorkModel             = "workModel";
    public const string EducationLevel        = "educationLevel";
    public const string EducationStatus       = "educationStatus";
    public const string AdditionalPaymentType    = "additionalPaymentType";
    public const string DeductionType            = "deductionType";
    public const string RelationType             = "relationType";
    public const string PayType                  = "payType";
    public const string PaymentPeriod            = "paymentPeriod";
    public const string AdditionalPaymentPeriod  = "additionalPaymentPeriod";
    public const string Currency                 = "currency";
    public const string BenefitType              = "benefitType";
    public const string PositionChangeType        = "positionChangeType";
}
public static class DomainOptionCatalog
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> ByCategory =
        new Dictionary<string, IReadOnlyList<string>>
        {
            [DomainCategories.ContractType]          = ContractTypes.All,
            [DomainCategories.WorkModel]             = WorkModels.All,
            [DomainCategories.EducationLevel]        = EducationLevels.All,
            [DomainCategories.EducationStatus]       = EducationStatuses.All,
            [DomainCategories.AdditionalPaymentType]   = AdditionalPaymentTypes.All,
            [DomainCategories.DeductionType]           = DeductionTypes.All,
            [DomainCategories.RelationType]            = RelationTypes.All,
            [DomainCategories.PayType]                 = PayTypes.All,
            [DomainCategories.PaymentPeriod]            = PaymentPeriods.All,
            [DomainCategories.AdditionalPaymentPeriod] = AdditionalPaymentPeriods.All,
            [DomainCategories.Currency]                = Currencies.All,
            [DomainCategories.BenefitType]              = BenefitTypes.All,
            [DomainCategories.PositionChangeType]        = PositionChangeTypes.All,
        };

    public static IReadOnlyList<string> Codes(string category)
        => ByCategory.TryGetValue(category, out var codes) ? codes : [];

    public static bool IsValid(string category, string? code)
        => code is not null && ByCategory.TryGetValue(category, out var codes) && codes.Contains(code);
}
