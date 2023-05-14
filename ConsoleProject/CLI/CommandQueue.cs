using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConsoleProject.CLI
{
    public class CommandQueue : Queue<QueueableCommand>, IXmlSerializable
    {
        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var command in this)
            {
                XmlSerializer serializer = new XmlSerializer(command.GetType());
                serializer.Serialize(writer, command);
            }
        }
    }
}
