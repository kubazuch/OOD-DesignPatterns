using System;
using System.IO;

namespace ConsoleProject.CLI.Arguments
{
    class PathArgument : CommandArgument<string>
    {
        private bool _mustExist;

        public PathArgument(bool required = false, string name = "filename", bool mustExist = false) : base(name, required)
        {
            _mustExist = mustExist;
        }

        public override string Parse(string arg)
        {
            var parent = Path.GetDirectoryName(arg);
            if (!string.IsNullOrEmpty(parent) && !Directory.Exists(parent))
                throw new ArgumentException($"Cannot open file at nonexistent directory `{parent}`");
            if (_mustExist && !File.Exists(arg))
                throw new ArgumentException($"Cannot open nonexistent file `{arg}`");

            return arg;
        }
    }
}
