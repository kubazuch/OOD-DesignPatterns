using ConsoleProject.CLI.Arguments;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

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

        public abstract void Process(string line, List<string> context);

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
