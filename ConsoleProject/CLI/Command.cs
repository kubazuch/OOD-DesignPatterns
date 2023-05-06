using ConsoleProject.CLI.Arguments;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ConsoleProject.CLI
{
    public class Command
    {
        public delegate void CommandDelegate(List<object> args = null);

        public string Name { get; }
        public string Description { get; }
        public string UsageDetails { get; }
        public CommandDelegate Delegate { get; }
        public ImmutableList<ICommandArgument> Args { get; }
        public ICommandArgument Vararg { get; }

        protected Command(string name, string description, string details, CommandDelegate command, IEnumerable<ICommandArgument> args, ICommandArgument vararg)
        {
            this.Name = name;
            this.Description = description;
            this.UsageDetails = details;
            this.Delegate = command;
            this.Args = args.ToImmutableList();
            this.Vararg = vararg;
        }

        public static CommandBuilder Named(string name) => new(name);

        public void Execute(DataManager data, List<string> context)
        {
            if (Vararg == null && Args.Count < context.Count)
                throw new ArgumentException($"Too many arguments for this command.\n\tUsage: {ToString()}");
            int i;
            var args = new List<object>();
            for (i = 0; i < Args.Count; ++i)
            {
                if (i >= context.Count)
                {
                    if (Args[i].Required)
                        throw new ArgumentException($"Missing required argument #{i + 1}: {Args[i].Name}.\n\tUsage: {ToString()}");
                    if (Args[i].IncludeRaw)
                        args.Add("");
                    args.Add(null);
                    continue;
                }

                if (Args[i].IncludeRaw)
                    args.Add(context[i]);
                args.Add(Args[i].Parse(data, context[i]));
            }

            if (Vararg != null)
            {
                for (; i < context.Count; ++i)
                {
                    if (Vararg.IncludeRaw)
                        args.Add(context[i]);
                    args.Add(Vararg.Parse(data, context[i]));
                }
            }

            Delegate(args);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name);
            foreach (var arg in Args)
            {
                builder.Append(' ').Append(arg);
            }

            if (Vararg != null)
            {
                builder.Append(' ').Append('[').Append(Vararg).Append(" ...]");
            }

            return builder.ToString();
        }

        public class CommandBuilder
        {
            private string _name;
            private string _description;
            private string _details;
            private CommandDelegate _delegate;
            private List<ICommandArgument> _args = new();
            private ICommandArgument _vararg;

            public string Name => _name;

            internal CommandBuilder(string name)
            {
                _name = name;
            }

            public CommandBuilder WithDescription(string desc)
            {
                _description = desc;
                return this;
            }

            public CommandBuilder WithUsageDetails(string details)
            {
                _details = details;
                return this;
            }

            public CommandBuilder Calls(CommandDelegate command)
            {
                _delegate = command;
                return this;
            }

            public CommandBuilder WithArg(ICommandArgument argument)
            {
                _args.Add(argument);
                return this;
            }

            public CommandBuilder WithVararg(ICommandArgument argument)
            {
                _vararg = argument;
                return this;
            }

            public Command Build() => new(_name, _description, _details, _delegate, _args, _vararg);
        }
    }
}
