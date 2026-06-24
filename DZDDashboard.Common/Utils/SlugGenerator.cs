using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DZDDashboard.Common.Utils;

public static partial class SlugGenerator
{
    private static readonly Dictionary<char, char> TurkishMap = new()
    {
        ['ç'] = 'c', ['ğ'] = 'g', ['ı'] = 'i', ['ö'] = 'o', ['ş'] = 's', ['ü'] = 'u',
        ['Ç'] = 'c', ['Ğ'] = 'g', ['İ'] = 'i', ['Ö'] = 'o', ['Ş'] = 's', ['Ü'] = 'u', ['I'] = 'i'
    };

    public static string FromName(string? first, string? last)
        => Slugify(AppFormatter.BuildFullName(first, last));

    public static string Slugify(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "user";

        var mapped = new StringBuilder(input.Length);
        foreach (var ch in input)
            mapped.Append(TurkishMap.TryGetValue(ch, out var rep) ? rep : ch);

        var lowered = RemoveDiacritics(mapped.ToString().ToLowerInvariant());

        var sb = new StringBuilder(lowered.Length);
        foreach (var ch in lowered)
        {
            if (ch is >= 'a' and <= 'z' or >= '0' and <= '9') sb.Append(ch);
            else if (ch is ' ' or '-' or '_' or '.') sb.Append('-');
        }

        var slug = MultiDashRegex().Replace(sb.ToString(), "-").Trim('-');
        return string.IsNullOrEmpty(slug) ? "user" : slug;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var ch in normalized)
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    [GeneratedRegex("-+")]
    private static partial Regex MultiDashRegex();
}
