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
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.IsStartElement())
                {
                    string elementName = reader.Name;
                    reader.ReadStartElement();
                    string commandName = char.ToLower(elementName[0]) + elementName.Substring(1, elementName.IndexOf("Command", StringComparison.Ordinal) - 1);
                    QueueableCommand command = App.Instance._commandDispatcher.GetCommandClone(commandName);
                    command.ReadXml(reader);
                    Enqueue(command);
                    reader.ReadEndElement();
                    reader.MoveToContent();
                }
                else
                {
                    reader.Read();
                }
            }

            reader.ReadEndElement();
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
