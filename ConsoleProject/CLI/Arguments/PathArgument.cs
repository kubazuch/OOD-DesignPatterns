using System;
using System.IO;

namespace ConsoleProject.CLI.Arguments
{
    class PathArgument : CommandArgument<string>
    {
        public PathArgument(bool required = false, string name = "filename")
        {
            this.Required = required;
            this.Name = name;
        }

        public string Parse(string arg, bool exist = false)
        {
            var parent = Path.GetDirectoryName(arg);
            if (!string.IsNullOrEmpty(parent) && !Directory.Exists(parent))
                throw new ArgumentException($"Cannot open file at nonexistent directory `§l{parent}§r`");
            if (exist && !File.Exists(arg))
                throw new ArgumentException($"Cannot open nonexistent file `§l{arg}§r`");

            return arg;
        }
    }
}
