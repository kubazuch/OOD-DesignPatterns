using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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
        private (List<string> raw, Predicate<object> parsed) _predicate;

        public FindCommand() : base("", "") => throw new NotImplementedException();

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

        public override void Process(List<string> raw, List<string> context, TextReader source, bool silent = false)
        {
            if (context.Count == 0)
                throw new MissingArgumentException(this, 1, TypeArg.Name);

            _collection = (context[0], TypeArg.Parse(_data, context[0]));

            List<Predicate<Entity>> predicates = new List<Predicate<Entity>>();
            for (int i = 1; i < context.Count; ++i)
            {
                predicates.Add(PredicateArg.Parse(context[0], context[i]));
            }

            _predicate = (raw.Skip(2).ToList(), entity => predicates.All(predicate => predicate((Entity)entity)));

            Line = string.Join(' ', raw);
            Cloned = true;
        }

        public override void Execute() => Algorithms.Print(_collection.parsed, _predicate.parsed);

        public override string ToHumanReadableString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").AppendLine(_collection.raw);
            sb.Append("§rpredicate=§e").Append(string.Join("§r and §e", _predicate.raw)).Append("§r");

            return sb.ToString();
        }

        public override void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("Type");
            var type = reader.ReadContentAsString();
            reader.ReadEndElement();
            _collection = (type, TypeArg.Parse(_data, type));

            List<Predicate<Entity>> predicates = new List<Predicate<Entity>>();
            List<string> raw = new List<string>();
            while (reader.IsStartElement() && reader.Name == "Predicate")
            {
                reader.ReadStartElement();
                string pred = reader.ReadContentAsString();

                predicates.Add(PredicateArg.Parse(type, pred.RemoveQuotes()));
                raw.Add(pred);

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            _predicate = (raw, entity => predicates.All(predicate => predicate((Entity)entity)));

            Line = $"find {type} {string.Join(' ', raw)}";
            Cloned = true;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Type");
            writer.WriteValue(_collection.raw);
            writer.WriteEndElement();

            foreach (var predicate in _predicate.raw)
            {
                writer.WriteStartElement("Predicate");
                writer.WriteValue(predicate);
                writer.WriteEndElement();
            }
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
