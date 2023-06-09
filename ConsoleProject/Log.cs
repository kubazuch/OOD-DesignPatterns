﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using ConsoleProject.CLI;

namespace ConsoleProject
{
    public static class Log
    {
        private static readonly Regex Regex = new(@"(§[0-9a-flr])", RegexOptions.Compiled);
        private static readonly Regex Bold = new(@"(?<!\\)\*(.*?[^\\])\*", RegexOptions.Compiled);
        private static readonly Regex Emph = new(@"(?<!\\)`(.*?[^\\])`", RegexOptions.Compiled);

        public static void Write(StreamWriter writer, string s)
        {
            var split = Regex.Split(s);
            foreach (var item in split)
            {
                if (item.Length != 2 || item[0] != '§')
                {
                    writer.Write(item);
                }
            }
        }

        public static void WriteLine(StreamWriter writer, string s = "")
        {
            Write(writer,s + "\n");
        }

        public static void Write(string s, bool formatted = true)
        {
            if (formatted)
            {
                s = Bold.Replace(s, match => $"§l{match.Groups[1].Value}§l");
                s = Emph.Replace(s, match => $"`§l{match.Groups[1].Value}§l`");
                s = s.Replace("\\*", "*").Replace("\\`", "`");
            }

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

        public static void WriteLine(string s = "", bool formatted = true)
        {
            Write(s + "\n");
        }

        public static void HandleException(Exception ex)
        {
            using ((TemporaryConsoleColor)ConsoleColor.DarkRed)
            {
                Log.WriteLine(ex.Message);
                while (ex.InnerException != null)
                {
                    Log.WriteLine($"Caused by: {ex.InnerException.Message}");
                    ex = ex.InnerException;
                }
            }
        }
    }
}
