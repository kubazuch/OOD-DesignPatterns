using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleProject.CLI
{
    public abstract class Command
    {
        public string Name { get; }
        public string Description { get; }
        public string Line { get; protected set; }

        protected Command(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract void Process(List<string> raw, List<string> context, TextReader source, bool silent = false);

        public override string ToString() => Line;

        public virtual string ToHumanReadableString() => ToString();

        public virtual void PrintHelp(List<string>? o = null)
        {
            StringBuilder sb = new();
            sb.Append("§2").Append(Name).Append("§r\t").Append(Description);
            Log.WriteLine(sb.ToString());
        }
    }
}
