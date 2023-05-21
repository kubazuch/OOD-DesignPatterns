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
    public class AddCommand : Command
    {
        private NamedCollection _collection;
        private AbstractFactory _factory;
        private AbstractBuilder _builder;

        public AddCommand() : base("add") {}

        public AddCommand(NamedCollection collection, AbstractFactory factory, AbstractBuilder builder) : base("add")
        {
            _collection = collection;
            _factory = factory;
            _builder = builder;
        }

        public static Command? AddCall(List<object?> args, TextReader reader, bool verbose)
        {
            var collection = (NamedCollection)args[0]!;
            var factory = (AbstractFactory)args[1]!;
            var builder = AbstractBuilder.GetByType(collection.Name);

            if (verbose)
            {
                Log.WriteLine($"Creating new §l{collection.Name}§r");
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
                        Log.WriteLine($"§eCreation of {collection.Name} terminated.");
                    return null;
                }
                else
                {
                    Log.WriteLine($"§4Unknown subcommand: `§l{cmd}§4`. Possible subcommands: §cEXIT, DONE and assignments of form: field=value");
                }
            } while (true);

            return new AddCommand(collection, factory, builder);
        }

        public override void Execute()
        {
            Entity created = _factory.CreateEntity(_builder!);
            created.SetVault(App.Instance.DataManager.Vault);
            _collection.Add(created);
            Log.WriteLine($"§aCreated new §l{_collection.Name}§a:");
            Log.WriteLine(created.ToString());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").Append(_collection.Name).AppendLine("§r");
            sb.Append("representation=§e").Append(_factory.Name).AppendLine("§r");
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
            _builder = AbstractBuilder.GetByType(_collection.Name);

            reader.MoveToAttribute("representation");
            var rep = reader.GetAttribute("representation")!;
            _factory = AbstractFactory.Mapping[rep];

            if (empty)
                return;
            reader.Read();
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
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", _collection.Name);
            writer.WriteAttributeString("representation", _factory.Name);

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
            sb.Append(Name).Append(' ').Append(_collection.Name).Append(' ').AppendLine(_factory.Name);
            sb.AppendLine(_builder.ToString());
            sb.Append("DONE");

            return sb.ToString();
        }
    }
}
