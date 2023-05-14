using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleProject
{
    public static class Log
    {
        private static readonly Regex Regex = new(@"(§[0-9a-flr])", RegexOptions.Compiled);

        public static void Write(string s)
        {
            var split = Regex.Split(s);
            var old = Console.ForegroundColor;
            foreach (var item in split)
            {
                if (item.Length == 2 && item[0] == '§')
                {
                    Console.ForegroundColor = item[1] switch
                    {
                        'r' => old,
                        'l' => (ConsoleColor) ((int) Console.ForegroundColor ^ 8),
                        _ => (ConsoleColor) Convert.ToInt32(item[1].ToString(), 16)
                    };
                }
                else
                {
                    Console.Write(item);
                }
            }
            Console.ForegroundColor = old;
        }

        public static void WriteLine(string s = "")
        {
            Write(s + "\n");
        }
    }
}
