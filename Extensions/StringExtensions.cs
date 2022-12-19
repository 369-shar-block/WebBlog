using System.Globalization;
using System.Text.RegularExpressions;

namespace WebBlog.Extensions
{
    public static class StringExtensions
    {
        public static string Slugify(this string phrase)
        {
            // Remove all accents and make the string lower case. / To Lower Case?!
            string output = phrase.RemoveAccents().ToLower();

            // Remove Special chat
            output = Regex.Replace(output, @"[^A-Za-z0-9\s]", "");

            // Remove all additiional spaces in favour of just one.
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.
            return output;
        }

        private static string RemoveAccents(this string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                return phrase;
            }

            // Convert for Unicode
            phrase = phrase.Normalize(System.Text.NormalizationForm.FormD);

            // Format unicoode/ascii
            char[] chars = phrase.Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                                       != UnicodeCategory.NonSpacingMark).ToArray();

            // Convert and return the new phrase
            return new string(chars).Normalize(System.Text.NormalizationForm.FormD);
        }
    }
}
