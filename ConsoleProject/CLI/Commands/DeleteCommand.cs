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
    public class DeleteCommand : QueueableCommand
    {
        private static readonly TypeArgument TypeArg = new(true);
        private static readonly PredicateArgument PredicateArg = new(true);

        private readonly DataManager _data;

        private (string raw, ICollection parsed) _collection;
        private (List<string> raw, Predicate<object> parsed) _predicate;

        public DeleteCommand() : base("", "") => throw new NotImplementedException();

        public DeleteCommand(DataManager data)
            : base("delete", "Removes given record from collections")
        {
            _data = data;

            var sb = new StringBuilder();
            sb.Append(Name).Append(' ').Append(TypeArg).Append(" [").Append(PredicateArg).Append(" ...]");
            Line = sb.ToString();
        }

        private DeleteCommand(DeleteCommand other)
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

        public override void Execute()
        {
            Entity? entity = null;
            int count = 0;
            var iterator = _collection.parsed.GetForwardIterator();
            while (iterator.MoveNext())
            {
                if (!_predicate.parsed(iterator.Current)) continue;
                count++;
                entity ??= (Entity)iterator.Current;
            }

            if (count != 1)
                throw new ArgumentException($"Predicate `§l{string.Join("§r and §l", _predicate.raw)}§r` should specify one record uniquely, found: §l{count}");

            _collection.parsed.Delete(entity);

            Log.WriteLine($"§aRemoved §l{_collection.raw}§a:");
            Log.WriteLine(entity.ToString());
        }

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
            var type = reader.ReadContentAsString().RemoveQuotes();
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

            Line = string.Join(' ', raw);
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

        public override object Clone() => new DeleteCommand(this);

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine($"\nThis command allows deleting a given record if requirement conditions (which work the same as in the find and edit command) specify one record uniquely.");
        }
    }
}
