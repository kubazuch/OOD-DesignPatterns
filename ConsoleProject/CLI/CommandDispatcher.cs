using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ConsoleProject.CLI.Commands;

namespace ConsoleProject.CLI
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, Command> _registry;

        internal CommandQueue CommandQueue = new();

        public CommandDispatcher()
        {
            _registry = new Dictionary<string, Command>();

            Register(new HelpCommand(_registry));
        }

        public IEnumerable<string> GetRegisteredCommands() => _registry.Keys;
        public QueueableCommand GetCommandClone(string name) => (QueueableCommand) ((QueueableCommand)_registry[name]).Clone();

        public void Register(Command command)
        {
            if (_registry.ContainsKey(command.Name))
                throw new DuplicateNameException($"Command with name `§l{command.Name}§r` is already registered!");
            _registry[command.Name] = command;
        }

        public void Parse(string line, TextReader reader, bool silent = false)
        {
            if (line == "") return;

            List<string> result = new List<string>();
            List<string> rawResult = new List<string>();
            bool insideQuotes = false;
            StringBuilder current = new StringBuilder();
            StringBuilder rawCurrent = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                switch (c)
                {
                    case '"' when i == 0 || line[i - 1] != '\\':
                        insideQuotes = !insideQuotes;
                        rawCurrent.Append(c);
                        continue;
                    case ' ' when !insideQuotes:
                        {
                            if (current.Length > 0)
                            {
                                result.Add(current.ToString());
                                rawResult.Add(rawCurrent.ToString());
                                current = new StringBuilder();
                                rawCurrent = new StringBuilder();
                            }
                            continue;
                        }
                    default:
                        current.Append(c);
                        rawCurrent.Append(c);
                        break;
                }
            }

            if (current.Length > 0)
            {
                result.Add(current.ToString());
                rawResult.Add(rawCurrent.ToString());
            }

            result = result.Select(s => s.Replace("\\\"", "\"")).ToList();

            if (!_registry.TryGetValue(result[0], out var cmd))
            {
                throw new ArgumentException($"Unknown command: `§l{result[0]}§r`. Type `§lhelp§r` for help");
            }

            if (cmd is QueueableCommand queueableCmd)
            {
                var copy = (QueueableCommand) queueableCmd.Clone();
                copy.Process(rawResult, result.Skip(1).ToList(), reader, silent);
                
                if(copy.IsCloned())
                    CommandQueue.Enqueue(copy);
            }
            else
            {
                cmd.Process(rawResult, result.Skip(1).ToList(), reader, silent);
            }
        }
    }
}
