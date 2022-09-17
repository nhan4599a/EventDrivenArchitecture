using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Global.Shared.Extensions
{
    public static class StringExtensions
    {
        private static readonly string[] ENDING_CHARACTER_PLUARAL_ES = { "o", "s", "ch", "x", "sh", "z" };

        public static string ToSnakeCase(this string value)
        {
            return Regex.Replace(
                            value.ToCamelCase(),
                            "([a-z])([A-Z])",
                            "$1_$2",
                            RegexOptions.CultureInvariant,
                            TimeSpan.FromMilliseconds(100)
                        ).ToLowerInvariant();
        }

        public static string ToCamelCase(this string value)
        {
            return $"{value[..1].ToLowerInvariant()}{value[1..]}";
        }

        public static string ToPluralForm(this string value)
        {
            var lastChars = value[^2..];

            var correctPluralForm = ENDING_CHARACTER_PLUARAL_ES.Any(e => lastChars.EndsWith(e)) ? "es" : "s";

            return value + correctPluralForm;
        }
    }
}
