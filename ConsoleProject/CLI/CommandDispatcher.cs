using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConsoleProject.CLI.Commands;

namespace ConsoleProject.CLI
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, Command> _registry;

        public CommandDispatcher()
        {
            _registry = new Dictionary<string, Command>();

            Register(new HelpCommand(_registry));
        }

        public IEnumerable<string> GetRegisteredCommands() => _registry.Keys;

        public void Register(Command command)
        {
            if (_registry.ContainsKey(command.Name))
                throw new DuplicateNameException($"Command with name {command.Name} is already registered!");
            _registry[command.Name] = command;
        }

        public void Parse(string line)
        {
            if (line == "") return;

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

            if (!_registry.TryGetValue(result[0], out var cmd))
            {
                throw new ArgumentException($"Unknown command: {result[0]}. Type \"help\" for help");
            }

            if (cmd is QueueableCommand queueableCmd)
            {
                var copy = (QueueableCommand) queueableCmd.Clone();
                copy.Process(result.Skip(1).ToList());
                Console.WriteLine(copy);
                //copy.Execute();
            }
            else
            {
                cmd.Process(result.Skip(1).ToList());
            }
        }
    }
}
