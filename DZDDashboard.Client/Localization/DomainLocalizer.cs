using DZDDashboard.Common.Constants;

namespace DZDDashboard.Client.Localization;

/// <summary>A selectable option: the stored code plus its localized label.</summary>
public readonly record struct DomainOption(string Code, string Label);

public class DomainLocalizer(AppLocalizer loc)
{
    /// <summary>Localized label for a stored code (empty when null, code itself when unmapped).</summary>
    public string Label(string category, string? code)
        => string.IsNullOrEmpty(code) ? string.Empty : loc[Key(category, code)];

    public IReadOnlyList<DomainOption> Options(string category)
        => [.. DomainOptionCatalog.Codes(category).Select(c => new DomainOption(c, loc[Key(category, c)]))];

    private static string Key(string category, string code) => $"option.{category}.{code}";
}
