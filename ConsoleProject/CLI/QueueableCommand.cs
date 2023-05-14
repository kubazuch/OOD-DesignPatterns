using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConsoleProject.CLI
{
    public abstract class QueueableCommand : Command, ICloneable, IXmlSerializable
    {
        protected bool Cloned = false;

        protected QueueableCommand(string name, string description) 
            : base(name, description)
        {
        }

        public abstract object Clone();

        public bool IsCloned() => Cloned;

        public abstract void Execute();

        public abstract override string ToHumanReadableString();

        public override string ToString() => Line;

        public XmlSchema? GetSchema() => null;

        public abstract void ReadXml(XmlReader reader);

        public abstract void WriteXml(XmlWriter writer);
    }
}
