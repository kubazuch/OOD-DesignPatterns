using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleProject.CLI.Commands
{
    public class HelpCommand : Command
    {
        private readonly Dictionary<string, Command> _registry;

        public HelpCommand(Dictionary<string, Command> commandRegistry)
            : base("help", "Prints help information for commands.")
        {
            _registry = commandRegistry;
        }

        public override void Process(List<string> context)
        {
            if (context.Count == 0)
            {
                foreach (Command c in _registry.Values)
                    c.PrintHelp();
                return;
            }

            var command = context.First();
            if (!_registry.TryGetValue(command, out var cmd))
                throw new ArgumentException($"Unknown command: {command}. Type \"help\" for a list of commands");

            cmd.PrintHelp(context.Skip(1).ToList());
        }

        public override string ToString() => "help [command]";

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine("\nIf no `§lcommand§r` is given, prints all available commands with short descriptions. Otherwise, prints full `§lcommand§r` description and details.");
        }
    }
}
