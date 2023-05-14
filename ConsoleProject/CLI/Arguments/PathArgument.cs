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

        public string Parse(string arg)
        {
            var parent = Path.GetDirectoryName(arg);
            if (parent != "" && !Directory.Exists(parent))
                throw new ArgumentException($"Cannot create file at nonexistent directory `§l{parent}§r`");

            return arg;
        }
    }
}
