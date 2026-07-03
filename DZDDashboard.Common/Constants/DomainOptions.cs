namespace DZDDashboard.Common.Constants;

/// <summary>
/// Stable category keys for the code-embedded reference lists that replaced the
/// former parametric lookup tables. The same key drives client localization
/// (<c>option.&lt;category&gt;.&lt;code&gt;</c>) and server-side validation.
/// </summary>
public static class DomainCategories
{
    public const string ContractType          = "contractType";
    public const string WorkModel             = "workModel";
    public const string EducationLevel        = "educationLevel";
    public const string AdditionalPaymentType = "additionalPaymentType";
    public const string DeductionType         = "deductionType";
    public const string RelationType          = "relationType";
}

/// <summary>
/// Single source of truth mapping each domain category to its allowed codes.
/// Reuses the canonical constant lists so options, dropdowns and validation
/// stay in sync (DRY).
/// </summary>
public static class DomainOptionCatalog
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> ByCategory =
        new Dictionary<string, IReadOnlyList<string>>
        {
            [DomainCategories.ContractType]          = ContractTypes.All,
            [DomainCategories.WorkModel]             = WorkModels.All,
            [DomainCategories.EducationLevel]        = EducationLevels.All,
            [DomainCategories.AdditionalPaymentType] = AdditionalPaymentTypes.All,
            [DomainCategories.DeductionType]         = DeductionTypes.All,
            [DomainCategories.RelationType]          = RelationTypes.All,
        };

    public static IReadOnlyList<string> Codes(string category)
        => ByCategory.TryGetValue(category, out var codes) ? codes : [];

    public static bool IsValid(string category, string? code)
        => code is not null && ByCategory.TryGetValue(category, out var codes) && codes.Contains(code);
}
