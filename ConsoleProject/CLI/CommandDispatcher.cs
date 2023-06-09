﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Commands;

namespace ConsoleProject.CLI
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, CommandParser> _registry;

#if !HISTORY
        internal CommandQueue CommandQueue;
#else
        internal CommandHistory CommandHistory;
        internal Stack<Command> RedoStack;
#endif

        public CommandDispatcher()
        {
            _registry = new Dictionary<string, CommandParser>();
#if !HISTORY
            CommandQueue = new CommandQueue();
#else
            CommandHistory = new CommandHistory();
            RedoStack = new Stack<Command>();
#endif

            Register(CommandParser.New("help", "Prints help information for commands")
                .WithUsageDetails("If no `command` is given, prints all available commands with short descriptions. Otherwise, prints full `command` description and details.")
                .WithVararg(new CommandArgument<string>("command"))
                .Calls((args, _, _) =>
                {
                    if (args.Count == 0)
                    {
                        foreach (var c in _registry.Values)
                            Log.WriteLine($"§2{c.Name}\t§r{c.Description}");
                        return;
                    }

                    var cmd = (string)args[0]!;
                    if (!_registry.TryGetValue(cmd, out var command))
                        throw new ArgumentException($"Unknown command: {command}. Type \"help\" for a list of commands");

                    command.PrintHelp(args.Skip(1).Select(arg => (string) arg!).ToList());
                }));
        }

        public void Register(CommandParser.CommandParserBuilder commandBuilder) => Register(commandBuilder.Build());

        public void Register(CommandParser command)
        {
            if (_registry.ContainsKey(command.Name))
                throw new DuplicateNameException($"Command with name `{command.Name}` is already registered!");
            _registry[command.Name] = command;
        }

        public void Parse(string line, TextReader? reader = null, bool verbose = true)
        {
            reader ??= Console.In;

            if (line == "") return;

            var result = new List<string>();
            var insideQuotes = false;
            var current = new StringBuilder();
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
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
                result.Add(current.ToString());

            result = result.Select(s => s.Replace("\\\"", "\"")).ToList();

            if (!_registry.TryGetValue(result[0], out var cmd))
            {
                throw new ArgumentException($"Unknown command: `{result[0]}`. Type `help` for help");
            }

            var command = cmd.Execute(result.Skip(1).ToList(), reader, verbose);

            if (command == null) return;
#if !HISTORY
            CommandQueue.Enqueue(command);
#else
            command.Execute();
            RedoStack.Clear();
            CommandHistory.Push(command);
#endif
        }

#if HISTORY
        public void Undo()
        {
            if (CommandHistory.Count == 0)
            {
                Log.WriteLine("§eThere is nothing to undo.");
                return;
            }

            var command = CommandHistory.Pop();
            command.Undo();
            RedoStack.Push(command);
        }

        public void Redo()
        {
            if (RedoStack.Count == 0)
            {
                Log.WriteLine("§eThere is nothing to redo.");
                return;
            }

            var command = RedoStack.Pop();
            command.Redo();
            CommandHistory.Push(command);
        }
#endif
    }
}
