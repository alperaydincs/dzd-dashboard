using System.Globalization;

namespace DZDDashboard.Client.Localization;

public class AppLocalizer
{
    public const string DefaultCulture = "en";
    public static readonly string[] SupportedCultures = ["en", "tr"];

    public string this[string key]
    {
        get
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (AppStrings.Translations.TryGetValue(culture, out var table) && table.TryGetValue(key, out var value))
                return value;
            if (AppStrings.Translations.TryGetValue(DefaultCulture, out var fallback) && fallback.TryGetValue(key, out var fb))
                return fb;
            return key;
        }
    }
}
