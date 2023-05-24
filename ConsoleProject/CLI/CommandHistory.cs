using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ConsoleProject.CLI.Commands;

namespace ConsoleProject.CLI
{
    public class CommandHistory : Stack<Command>, IXmlSerializable
    {
        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                return;
            reader.Read();
            var i = 1;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.IsStartElement())
                {
                    var elementName = reader.Name;
                    var commandName = char.ToLower(elementName[0]) + elementName.Substring(1, elementName.IndexOf("Command", StringComparison.Ordinal) - 1);
                    var command = Command.Commands[commandName]();

                    var empty = reader.IsEmptyElement;
                    command.ReadXml(reader);
                    Log.WriteLine($"§5Command #{i++} ({command.Name}):");
                    command.Execute();
                    Log.WriteLine();
                    Push(command);
                    if (!empty)
                        reader.ReadEndElement();
                    else
                        reader.Read();

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
            foreach (var command in this.Reverse())
            {
                var serializer = new XmlSerializer(command.GetType());
                serializer.Serialize(writer, command);
            }
        }
    }
}
