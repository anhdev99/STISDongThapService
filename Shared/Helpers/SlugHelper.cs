using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Helpers;

public static class SlugHelper
{
    public static string GenerateSlug(string phrase)
    {
        // Convert to lowercase
        string str = phrase.ToLowerInvariant();

        // Remove diacritics (accents)
        str = RemoveDiacritics(str);

        // Replace spaces with hyphens
        str = Regex.Replace(str, @"\s+", "-");

        // Remove invalid characters (anything that is not a letter, number, or hyphen)
        str = Regex.Replace(str, @"[^a-z0-9\-]", "");

        // Trim hyphens from the beginning and end
        str = str.Trim('-');

        // Return the slug
        return str;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    } 
}