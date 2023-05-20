using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConsoleProject.CLI.Commands
{
    public abstract class Command : IXmlSerializable
    {
        public static Dictionary<string, Func<Command>> Commands = new()
        {
            ["list"] = () => new ListCommand(),
            ["find"] = () => new FindCommand(),
            ["add"] = () => new AddCommand(),
            ["edit"] = () => new EditCommand(),
            ["delete"] = () => new DeleteCommand()
        };

        public string Name { get; }

        protected Command(string name)
        {
            Name = name;
        }

        public abstract void Execute();

        public abstract string ToCommandline();

        public abstract override string ToString();

        public XmlSchema? GetSchema() => null;
        
        public abstract void ReadXml(XmlReader reader);

        public abstract void WriteXml(XmlWriter writer);
    }
}
