using System;
using System.Text;
using System.Xml;
using BTM.Collections;
using ConsoleProject.CLI.Arguments;

namespace ConsoleProject.CLI.Commands
{
    public class ListCommand : Command
    {
        private NamedCollection _collection;

        public ListCommand() : base("list") {}

        public ListCommand(NamedCollection collection)
            : base("list")
        {
            _collection = collection;
        }

        public override void Execute() => Algorithms.ForEach(_collection.GetForwardIterator(), Console.WriteLine);

#if HISTORY
        public override void Undo() { }

        public override void Redo() { }

#endif
        public override string ToCommandline() => $"{Name} {_collection.Name}";

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").Append(_collection.Name).Append("§r");

            return sb.ToString();
        }

        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToAttribute("type");
            var name = reader.GetAttribute("type")!;
            _collection = new NamedCollection(name, App.Instance.DataManager.Mapping[name]);
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", _collection.Name);
        }
    }
}
