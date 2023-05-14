using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM;
using BTM.Collections;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    public class FindCommand : QueueableCommand
    {
        private static readonly TypeArgument TypeArg = new(true);
        private static readonly PredicateArgument PredicateArg = new(true);

        private readonly DataManager _data;

        private (string raw, ICollection parsed) _collection;
        private (string raw, Predicate<object> parsed) _predicate;

        public FindCommand(DataManager data)
            : base("find", "Prints objects matching certain conditions")
        {
            _data = data;

            var sb = new StringBuilder();
            sb.Append(Name).Append(' ').Append(TypeArg).Append(" [").Append(PredicateArg).Append(" ...]");
            Line = sb.ToString();
        }

        private FindCommand(FindCommand other)
            : base(other.Name, other.Description)
        {
            _data = other._data;

            Line = other.Line;
        }

        public override void Process(string line, List<string> context)
        {
            if (context.Count == 0)
                throw new MissingArgumentException(this, 1, TypeArg.Name);

            _collection = (context[0], TypeArg.Parse(_data, context[0]));

            List<Predicate<Entity>> predicates = new List<Predicate<Entity>>();
            for (int i = 1; i < context.Count; ++i)
            {
                predicates.Add(PredicateArg.Parse(context[0], context[i]));
            }

            _predicate = ("§e" + string.Join("§r and §e", context.Skip(1)) + "§r", entity => predicates.All(predicate => predicate((Entity)entity)));

            Line = line;
            Cloned = true;
        }

        public override void Execute() => Algorithms.Print(_collection.parsed, _predicate.parsed);

        public override string ToHumanReadableString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").AppendLine(_collection.raw);
            sb.Append("§rpredicate=").Append(_predicate.raw);

            return sb.ToString();
        }

        public override object Clone() => new FindCommand(this);

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine($"\nwhere requirements (space separated list of requirements) specify acceptable values of atomic non reference fields. They follow format:");
            Log.WriteLine("\n\t§l<name_of_field>=|<|><value>");
            Log.WriteLine("\nwhere `§l=|<|>§r` means any strong comparison operator. For numerical fields natural comparison will be used. Strings will use a lexicographic order. For other types only `§l=§r` is allowed. If a value were to contain spaces it should be placed inside quotation marks.");
        }
    }
}
