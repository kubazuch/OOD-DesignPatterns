using System.Text.RegularExpressions;

namespace BTM
{
    public static class Extensions
    {
        private static readonly Regex Quote = new(@"(?<!\\)""", RegexOptions.Compiled);

        public static string RemoveQuotes(this string s)
        {
            s = Quote.Replace(s, "");
            return s.Replace("\\\"", "\"");
        }

        public static string Enquote(this string s) => s.Contains(' ') ? $"\"{s.Replace("\"", "\\\"")}\"" : s.Replace("\"", "\\\"");
    }
}
