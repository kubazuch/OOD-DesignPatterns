using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Commands;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI
{
    public class CommandParser
    {
        public delegate void VoidCommandDelegate(List<object?> args, TextReader reader, bool verbose);
        public delegate Command? CommandDelegate(List<object?> args, TextReader reader, bool verbose);

        public delegate List<object?> ParserDelegate(CommandParser self, List<string> args);

        public string Name { get; }
        public string Description { get; }
        public string? UsageDetails { get; }
        public CommandDelegate? Action { get; }

        public ParserDelegate? Parser { get; }
        public CommandParser? Parent { get; protected set; }
        public ImmutableDictionary<string, CommandParser> Subcommands { get; }
        public ICommandArgument[] Args { get; }
        public ICommandArgument? Vararg { get; }

        public bool HasArguments => Args.Length > 0 || Vararg != null;
        public bool HasSubcommands => Subcommands.Count > 0;

        public string FullName => Parent == null ? Name : $"{Parent.FullName} {Name}";

        private CommandParser(string name, string desc, string? details, CommandDelegate? action, ParserDelegate? parser,
            CommandParser[] subcommands, ICommandArgument[] args, ICommandArgument? vararg)
        {
            Name = name;
            Description = desc;
            UsageDetails = details;
            Parser = parser;
            Action = action;
            Subcommands = subcommands.ToImmutableDictionary(c => c.Name, c => c);
            Args = args;
            Vararg = vararg;

            foreach (var subcommand in subcommands)
                subcommand.Parent = this;
        }

        private List<object?> Parse(List<string> context)
        {
            if(Parser != null)
                return Parser(this, context);

            if (Vararg == null && context.Count > Args.Length)
                throw new TooManyArgumentsException(this);

            int i;
            var args = new List<object?>();
            for (i = 0; i < Args.Length; ++i)
            {
                if (i >= context.Count)
                {
                    if (Args[i].Required)
                        throw new MissingArgumentException(this, i + 1, Args[i].Name);
                    args.Add(null);
                    continue;
                }

                args.Add(Args[i].Parse(context[i]));
            }

            if (Vararg == null)
                return args;

            for (; i < context.Count; ++i)
                args.Add(Vararg.Parse(context[i]));

            return args;
        }

        public Command? Execute(List<string> context, TextReader? reader = null, bool verbose = true)
        {
            reader ??= Console.In;

            if (context.Count > 0 && Subcommands.TryGetValue(context[0], out var subCommand))
                return subCommand.Execute(context.Skip(1).ToList());

            if (Action == null)
            {
                if (context.Count == 0)
                    throw new ArgumentException($"Missing subcommand.\n\tUsage: §l{this}");

                throw new ArgumentException($"Invalid subcommand `{context[0]}`.\n\tUsage: §l{this}");
            }

            var args = Parse(context);


            return Action.Invoke(args, reader, verbose);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(FullName);

            if (HasArguments)
            {
                foreach (var commandArgument in Args)
                {
                    sb.Append(' ').Append(commandArgument);
                }

                if (Vararg != null)
                {
                    sb.Append(' ').Append('[').Append(Vararg).Append(" ...]");
                }
            }

            if (HasArguments && HasSubcommands)
                sb.Append($" §lOR§l {FullName}");

            if (HasSubcommands)
                sb.Append(' ').Append(string.Join('|', Subcommands.Select(kv => kv.Key))).Append(" ...");

            return sb.ToString();
        }

        public void PrintHelp(List<string> context)
        {
            if (context.Count > 0 && Subcommands.TryGetValue(context[0], out var subCommand))
            {
                subCommand.PrintHelp(context.Skip(1).ToList());
                return;
            }

            Log.WriteLine($"§2{FullName}\t§r{Description}");
            Log.WriteLine("Usage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine();

            if (HasSubcommands)
                Log.WriteLine($"§3This command has subcommands. For more info try: `help {FullName} {string.Join('|', Subcommands.Select(kv => kv.Key))}`\n");
            
            if(UsageDetails != null)
                Log.WriteLine(UsageDetails);
        }

        public static CommandParserBuilder New(string name, string desc) => new(name, desc);

        public class CommandParserBuilder
        {
            private string _name;
            private string _description;
            private string? _details;
            private CommandDelegate? _action;
            private ParserDelegate? _parser;
            private List<CommandParserBuilder> _subcommands;
            private List<ICommandArgument> _args;
            private ICommandArgument? _vararg;

            internal CommandParserBuilder(string name, string description)
            {
                _name = name;
                _description = description;
                _subcommands = new List<CommandParserBuilder>();
                _args = new List<ICommandArgument>();
            }

            public CommandParserBuilder WithUsageDetails(string details)
            {
                _details = details;
                return this;
            }

            public CommandParserBuilder WithSubcommand(CommandParserBuilder subcmd)
            {
                _subcommands.Add(subcmd);
                return this;
            }

            public CommandParserBuilder WithSubcommands(params CommandParserBuilder[] subcmds)
            {
                _subcommands.AddRange(subcmds);
                return this;
            }

            public CommandParserBuilder WithArg(ICommandArgument argument)
            {
                _args.Add(argument);
                return this;
            }

            public CommandParserBuilder WithArgs(params ICommandArgument[] arguments)
            {
                _args.AddRange(arguments);
                return this;
            }

            public CommandParserBuilder WithVararg(ICommandArgument argument)
            {
                _vararg = argument;
                return this;
            }

            public CommandParserBuilder Calls(CommandDelegate action)
            {
                _action = action;
                return this;
            }

            public CommandParserBuilder WithParser(ParserDelegate parser)
            {
                _parser = parser;
                return this;
            }

            public CommandParserBuilder Calls(VoidCommandDelegate action)
            {
                _action = ( args, reader, verbose) =>
                {
                    action(args, reader, verbose);
                    return null;
                };

                return this;
            }

            public CommandParser Build()
            {
                if (_action == null && _subcommands.Count == 0)
                    throw new ArgumentException("Command must have callback function or subcommands");

                if (_action == null && (_args.Count > 0 || _vararg != null))
                    throw new ArgumentException("Command has arguments so it must have callback function");

                return new CommandParser(_name, _description, _details, _action, _parser,
                    _subcommands.Select(subcmd => subcmd.Build()).ToArray(), _args.ToArray(), _vararg);
            }
        }
    }
}
