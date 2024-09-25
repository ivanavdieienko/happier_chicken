using System.Collections.Generic;

public static class Localization
{
    public const string I = "أنا ♥";
    public const string Results = "{0} بيض = {1} عملات ذهبية";

    public static string[] Leaders = { "محمد", "علي", "عمر", "حسن", "كريم", "فاطمة", "عائشة", "ليلى", "سعيد", "إبراهيم" };

    private static Dictionary<string, string> defaultLocale;
    private static Dictionary<string, string> currentLocale;
}