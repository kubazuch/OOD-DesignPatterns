using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BTM;
using ConsoleProject.CLI.Arguments;

namespace ConsoleProject.CLI.Commands
{
    public class DeleteCommand : Command
    {
        private NamedCollection _collection;
        private List<EntityPredicate> _predicates;

        private Predicate<object> _predicate;

        public DeleteCommand()  : base("delete") {}

        public DeleteCommand(NamedCollection collection, List<EntityPredicate> predicates) : base("delete")
        {
            _collection = collection;
            _predicates = predicates;
            _predicate = entity => _predicates.All(pred => pred.Predicate((Entity)entity));
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

            _collection.Delete(entity);

            Log.WriteLine($"§aRemoved §l{_collection.Name}§a");

            entity.Dispose();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").AppendLine(_collection.Name);
            sb.Append("§rpredicate=§e").Append(string.Join("§r and §e", _predicates)).Append("§r");

            return sb.ToString();
        }

        public override void ReadXml(XmlReader reader)
        {
            bool empty = reader.IsEmptyElement;
            reader.MoveToAttribute("type");
            var name = reader.GetAttribute("type")!;
            _collection = new NamedCollection(name, App.Instance.DataManager.Mapping[name]);
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
        }

        public override string ToCommandline() => $"{Name} {_collection.Name} {string.Join(' ', _predicates)}";
    }
}
