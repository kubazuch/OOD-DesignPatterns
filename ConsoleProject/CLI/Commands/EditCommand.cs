
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using BTM;
using BTM.Builder;
using BTM.Refraction;
using ConsoleProject.CLI.Arguments;

namespace ConsoleProject.CLI.Commands
{
    public class EditCommand : Command
    {
        private NamedCollection _collection;
        private List<EntityPredicate> _predicates;
        private AbstractBuilder _builder;

        private Predicate<object> _predicate;

        public EditCommand() : base("edit") {}

        public EditCommand(NamedCollection collection, List<EntityPredicate> predicates, AbstractBuilder builder) : base("edit")
        {
            _collection = collection;
            _predicates = predicates;
            _builder = builder;

            _predicate = entity => _predicates.All(pred => pred.Predicate((Entity)entity));
        }

        public static Command? EditCall(List<object?> args, TextReader reader, bool verbose)
        {
            var collection = (NamedCollection)args[0]!;
            var builder = AbstractBuilder.GetByType(collection.Name, false);
            var predicates = args.Skip(1).Select(x => (EntityPredicate)x!).ToList();

            if (verbose)
            {
                Log.WriteLine($"Editing new §l{collection.Name}§r");
                Log.WriteLine($"§3Available fields: §l{string.Join(", ", builder.Fields.Keys)}");
            }

            do
            {
                if (verbose)
                    Log.Write("§e+ ");

                var cmd = reader.ReadLine().Trim();
                if (cmd == "")
                    continue;

                var match = App.Assignment.Match(cmd);
                if (match.Success)
                {
                    var field = match.Groups[1].Value;
                    var val = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value;

                    try
                    {
                        ((IRefractive)builder)[field] = val;
                    }
                    catch (ArgumentException ex)
                    {
                        Log.HandleException(ex);
                    }
                }
                else if (cmd is "DONE")
                    break;
                else if (cmd is "EXIT")
                {
                    if (verbose)
                        Log.WriteLine($"§eEdition of {collection.Name} terminated.");
                    return null;
                }
                else
                {
                    Log.WriteLine($"§4Unknown subcommand: `§l{cmd}§4`. Possible subcommands: §cEXIT, DONE and assignments of form: field=value");
                }
            } while (true);

            return new EditCommand(collection, predicates, builder);
        }

        public override void Execute()
        {
            Entity? entity = null;
            int count = 0;
            var iterator = _collection.GetForwardIterator();
            while (iterator.MoveNext())
            {
                if (!_predicate(iterator.Current)) continue;
                count++;
                entity ??= (Entity)iterator.Current;
            }

            if (count != 1)
                throw new ArgumentException($"Predicate `§l{string.Join("§l and §l", _predicates)}§l` should specify one record uniquely, found: §l{count}");

            foreach (var field in _builder.Fields.Values.Where(field => field.Value != null))
            {
                ((IRefractive)entity!)[field.Name] = field.Value;
            }

            Log.WriteLine($"§aEdited §l{_collection.Name}§a:");
            Log.WriteLine(entity.ToString());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").AppendLine(_collection.Name);
            sb.Append("§rpredicate=§e").Append(string.Join("§r and §e", _predicates)).AppendLine("§r");
            foreach (var field in _builder.Fields.Where(field => field.Value.Value != null))
            {
                sb.Append(field.Key).Append("=§e").Append(field.Value.Value.ToString()!.Enquote()).AppendLine("§r");
            }

            return sb.ToString().TrimEnd();
        }

        public override void ReadXml(XmlReader reader)
        {
            bool empty = reader.IsEmptyElement;
            reader.MoveToAttribute("type");
            var name = reader.GetAttribute("type")!;
            _collection = new NamedCollection(name, App.Instance.DataManager.Mapping[name]);
            _builder = AbstractBuilder.GetByType(_collection.Name, false);
            reader.Read();

            _predicates = new List<EntityPredicate>();
            if (empty)
            {
                _predicate = entity => _predicates.All(pred => pred.Predicate((Entity)entity));
                return;
            }

            reader.Read();
            while (reader.IsStartElement() && reader.Name == "Predicate")
            {
                var field = reader.GetAttribute("field")!;
                var cmp = reader.GetAttribute("comparison")!;
                reader.ReadStartElement();
                string pred = reader.ReadContentAsString();

                _predicates.Add(new EntityPredicate(_collection.Name, field, cmp, pred));

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.IsStartElement())
                {
                    string elementName = reader.Name;
                    reader.ReadStartElement();
                    string fieldName = char.ToLower(elementName[0]) + elementName[1..];
                    string fieldVal = reader.ReadContentAsString();

                    ((IRefractive)_builder)[fieldName] = fieldVal.RemoveQuotes();

                    reader.ReadEndElement();
                    reader.MoveToContent();
                }
                else
                {
                    reader.Read();
                }
            }

            _predicate = entity => _predicates.All(pred => pred.Predicate((Entity)entity));
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", _collection.Name);

            foreach (var predicate in _predicates)
            {
                writer.WriteStartElement("Predicate");
                predicate.WriteXml(writer);
                writer.WriteEndElement();
            }

            foreach (var field in _builder.Fields.Where(field => field.Value.Value != null))
            {
                writer.WriteStartElement(char.ToUpper(field.Key[0]) + field.Key[1..]);
                writer.WriteValue(field.Value.Value);
                writer.WriteEndElement();
            }
        }

        public override string ToCommandline()
        {
            var sb = new StringBuilder();
            sb.Append(Name).Append(' ').Append(_collection.Name).Append(' ').AppendLine(string.Join(' ', _predicates));
            sb.AppendLine(_builder.ToString());
            sb.Append("DONE");

            return sb.ToString();
        }
    }
}
