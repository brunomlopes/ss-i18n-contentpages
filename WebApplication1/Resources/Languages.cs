using System;
using System.Collections.Generic;
using System.Globalization;

namespace WebApplication1.Resources
{
    public class Languages
    {
        private static Dictionary<string, CultureInfo> _SupportedLanguages = new Dictionary<string, CultureInfo>()
        {
            {"pt", CultureInfo.CreateSpecificCulture("pt")},
            {"en", CultureInfo.CreateSpecificCulture("en")},
        };

        public static string DefaultLanguage = "en";

        public static CultureInfo ReduceToSupportedLanguage(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)) throw new ArgumentNullException("languageCode");

            if (_SupportedLanguages.ContainsKey(languageCode)) return _SupportedLanguages[languageCode];
            var fallback = languageCode.Split(new[] { '-' }, 1)[0];
            if (_SupportedLanguages.ContainsKey(fallback)) return _SupportedLanguages[fallback];
            return _SupportedLanguages[DefaultLanguage];
        }
    }
}