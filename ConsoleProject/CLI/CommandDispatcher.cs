using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleProject.CLI
{
    public class CommandDispatcher
    {
        public static readonly string Separator = " ";
        private static readonly Regex Regex = new (@"[\""].+?[\""]|[^ ]+", RegexOptions.Compiled);
        private readonly Dictionary<string, Command> _registry;
        private readonly DataManager _data;

        public CommandDispatcher(DataManager data)
        {
            this._data = data;
            this._registry = new Dictionary<string, Command>();

            Register(Command.Named("help").WithDescription("Prints help information for all commands.").Calls(_ =>
            {
                foreach (Command c in _registry.Values)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(c.Name);
                    Console.ResetColor();
                    Console.WriteLine($"\t{c.Description}");
                    Console.Write("Usage: \n\t");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(c.ToString());
                    Console.ResetColor();
                    if(c.UsageDetails != null)
                        Console.WriteLine('\n' + c.UsageDetails);
                    Console.WriteLine();
                }
            }));
        }

        public void Register(Command command)
        {
            if (this._registry.ContainsKey(command.Name))
                throw new DuplicateNameException($"Command with name {command.Name} is already registered!");
            this._registry[command.Name] = command;
        }

        public void Register(Command.CommandBuilder command)
        {
            Register(command.Build());
        }

        public void Parse(string line)
        {
            if(line == "") return;

            List<string> result = new List<string>();
            bool insideQuotes = false;
            StringBuilder current = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                switch (c)
                {
                    case '"' when i == 0 || line[i - 1] != '\\':
                        insideQuotes = !insideQuotes;
                        continue;
                    case ' ' when !insideQuotes:
                    {
                        if (current.Length > 0)
                        {
                            result.Add(current.ToString());
                            current = new StringBuilder();
                        }
                        continue;
                    }
                    default:
                        current.Append(c);
                        break;
                }
            }

            if (current.Length > 0)
            {
                result.Add(current.ToString());
            }

            result = result.Select(s => s.Replace("\\\"", "\"")).ToList();

            if (!_registry.TryGetValue(result[0], out Command cmd))
            {
                throw new ArgumentException($"Unknown command: {result[0]}. Type \"help\" for help");
            }

            cmd.Execute(_data, result.Skip(1).ToList());
        }
    }
}
