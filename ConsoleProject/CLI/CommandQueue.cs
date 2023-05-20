using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ConsoleProject.CLI.Commands;

namespace ConsoleProject.CLI
{
    public class CommandQueue : Queue<Command>, IXmlSerializable
    {
        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            //while (reader.Read())
            //{
            //    switch (reader.NodeType)
            //    {
            //        case XmlNodeType.Element:
            //            Console.Write("<" + reader.Name);

            //            if (reader.HasAttributes)
            //            {
            //                while (reader.MoveToNextAttribute())
            //                {
            //                    Console.Write(" " + reader.Name + "=\"" + reader.Value + "\"");
            //                }

            //                reader.MoveToElement();
            //            }

            //            if (reader.IsEmptyElement)
            //            {
            //                Console.WriteLine("/>");
            //            }
            //            else
            //            {
            //                Console.WriteLine(">");
            //            }
            //            break;

            //        case XmlNodeType.Text:
            //            Console.WriteLine(reader.Value);
            //            break;

            //        case XmlNodeType.EndElement:
            //            Console.WriteLine("</" + reader.Name + ">");
            //            break;

            //        case XmlNodeType.XmlDeclaration:
            //        case XmlNodeType.ProcessingInstruction:
            //            Console.WriteLine("<?" + reader.Name + " " + reader.Value + "?>");
            //            break;

            //        case XmlNodeType.Comment:
            //            Console.WriteLine("<!--" + reader.Value + "-->");
            //            break;

            //        case XmlNodeType.DocumentType:
            //            Console.WriteLine("<!DOCTYPE " + reader.Name + " [" + reader.Value + "]>");
            //            break;
            //    }
            //}
            if (reader.IsEmptyElement)
                return;
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.IsStartElement())
                {
                    string elementName = reader.Name;
                    string commandName = char.ToLower(elementName[0]) + elementName.Substring(1, elementName.IndexOf("Command", StringComparison.Ordinal) - 1);
                    Command command = Command.Commands[commandName]();

                    bool empty = reader.IsEmptyElement;
                    command.ReadXml(reader);
                    Enqueue(command);
                    if(!empty)
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
            foreach (var command in this)
            {
                XmlSerializer serializer = new XmlSerializer(command.GetType());
                serializer.Serialize(writer, command);
            }
        }
    }
}
