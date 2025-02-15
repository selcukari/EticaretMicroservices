using System.Net;
using System.Text.RegularExpressions;

namespace FreeCourse.Services.PhotoStock.Utils
{
    public static class HttpExtensions
    {
        public static string HtmlClear(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            var objRegExp = new Regex("<(.|\n)+?>");
            return objRegExp.Replace(html, String.Empty).Replace("\n", "").Replace("\r", "");
        }

        public static string ToLink(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var chars = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            var change = value.HtmlClear().Trim().ToLower();

            for (var i = 0; i < chars.Length; i++)
            {
                var strChar = chars.GetValue(i).ToString();
                if (change.Contains(chars.GetValue(i).ToString()))
                    change = change.Replace(strChar, newValue: String.Empty);
            }
            change = change.Replace("\t", " ");
            change = change.Replace("   ", " ");
            change = change.Replace("  ", " ");
            change = change.Replace(" ", "-");
            change = change.Replace("ç", "c");
            change = change.Replace("ğ", "g");
            change = change.Replace("ı", "i");
            change = change.Replace("ö", "o");
            change = change.Replace("ş", "s");
            change = change.Replace("ü", "u");
            change = change.Replace("…", "");
            change = change.Replace("..", "");
            change = change.Replace(".", "");
            change = change.Replace("‘", "");
            change = change.Replace("’", "");
            change = change.Replace("’", "");
            change = change.Replace("“", "");
            change = change.Replace("”", "");
            change = change.Replace("û", "u");
            change = change.Replace("â", "a");
            change = change.Replace("’", "");
            change = change.Replace("–", "-");
            change = change.Replace("​​", "").Trim();
            change = Regex.Replace(change, @"[^a-z0-9-_]", "");
            change = WebUtility.UrlEncode(change.ToLower().Trim());
            if (change != null)
            {
                change = change.Replace("%e2%80%8b", "").Replace("%e2", "").Replace("%80", "").Replace("%8b", "").Replace("&#8203;", "");
                change = change.Replace("\u8203", "").Trim();
                change = WebUtility.UrlDecode(change);
                change = change.Replace("�", "");

                return change;
            }
            return null;
        }
    }
}
