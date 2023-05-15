using System.Text.RegularExpressions;

namespace ConsoleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            App.Instance.Start();
        }

    }


    public static class Extensions
    {
        private static readonly Regex Quote = new(@"(?<!\\)""", RegexOptions.Compiled);
        public static string RemoveQuotes(this string s)
        {
            s = Quote.Replace(s, "");
            return s.Replace("\\\"", "\"");
        }
    }
}
