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
        }

        public void Register(Command command)
        {
            if (this._registry.ContainsKey(command.Name))
                throw new DuplicateNameException("Command with this name already registered!");
            this._registry[command.Name] = command;
        }

        public void Register(Command.CommandBuilder command)
        {
            if (this._registry.ContainsKey(command.Name))
                throw new DuplicateNameException("Command with this name already registered!");
            this._registry[command.Name] = command.Build();
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
                if (c == '"' && (i == 0 || line[i - 1] != '\\'))
                {
                    insideQuotes = !insideQuotes;
                    continue;
                }
                if (c == ' ' && !insideQuotes)
                {
                    if (current.Length > 0)
                    {
                        result.Add(current.ToString());
                        current = new StringBuilder();
                    }
                    continue;
                }
                current.Append(c);
            }
            if (current.Length > 0)
            {
                result.Add(current.ToString());
            }

            result = result.Select(s => s.Replace("\\\"", "\"")).ToList();

            try
            {
                Command cmd = _registry[result[0]];
                cmd.Execute(_data, result.Skip(1).ToList());
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"Unknown command: {result[0]}");
            }
        }
    }
}
