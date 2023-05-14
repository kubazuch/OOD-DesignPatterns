using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BTM;
using BTM.BaseData;
using BTM.Builder;
using BTM.Collections;
using BTM.Refraction;
using BTM.TupleStackData;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    public class AddCommand : QueueableCommand
    {
        private static readonly TypeArgument TypeArg = new(true);
        private static readonly EnumArgument<AbstractFactory> FactoryArg = new(new Dictionary<string, AbstractFactory> { ["base"] = new BaseAbstractFactory(), ["secondary"] = new TupleStackAbstractFactory() }, "representation", true);
        public static readonly Regex Assignment = new(@"^([^=]+?)\s*?=\s*?(?:([^""\s]+)|""([^""]+)"")", RegexOptions.Compiled);

        private readonly DataManager _data;

        private (string raw, ICollection parsed) _collection;
        private (string raw, AbstractFactory parsed) _factory;
        private AbstractBuilder? _builder;

        public AddCommand() : base("", "") => throw new NotImplementedException();

        public AddCommand(DataManager data)
            : base("add", "Adds a new object of a particular type")
        {
            _data = data;

            var sb = new StringBuilder();
            sb.Append(Name).Append(' ').Append(TypeArg).Append(' ').Append(FactoryArg);
            Line = sb.ToString();
        }

        private AddCommand(AddCommand other)
            : base(other.Name, other.Description)
        {
            _data = other._data;

            Line = other.Line;
        }

        public override void Process(string line, List<string> context)
        {
            if (context.Count == 0)
                throw new MissingArgumentException(this, 1, TypeArg.Name);

            _collection = (context[0], TypeArg.Parse(_data, context[0]));

            if (context.Count == 1)
                throw new MissingArgumentException(this, 2, FactoryArg.Name);

            _factory = (context[1], FactoryArg.Parse(context[1]));

            if (context.Count > 2)
                throw new TooManyArgumentsException(this);
            
            _builder = AbstractBuilder.GetByType(_collection.raw);

            Log.WriteLine($"Creating new §l{_collection.raw}§r");
            Log.WriteLine($"§3Available fields: §l{string.Join(", ", _builder.Fields.Keys)}");

            do
            {
                Log.Write("§e+ ");
                var cmd = Console.ReadLine().Trim();
                var match = Assignment.Match(cmd);

                if (cmd == "")
                    continue;
                else if (match.Success)
                {
                    var field = match.Groups[1].Value;
                    var val = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value;

                    try
                    {
                        ((IRefractive) _builder).SetValueByName(field, val);
                    }
                    catch (ArgumentException ex)
                    {
                        using ((TemporaryConsoleColor)ConsoleColor.DarkRed)
                        {
                            Log.WriteLine(ex.Message);
                            if (ex.InnerException != null)
                                Log.WriteLine($"Caused by: {ex.InnerException.Message}");
                        }
                    }
                }
                else if (cmd is "DONE")
                    break;
                else if (cmd is "EXIT")
                {
                    Log.WriteLine($"§eCreation of {_collection.raw} terminated.");
                    return;
                }
                else
                { 
                    Log.WriteLine($"§4Unknown subcommand: `§l{cmd}§4`. Possible subcommands: §cEXIT, DONE and assigmnents of form: field=value");
                }
            } while (true);

            var sb = new StringBuilder();
            sb.AppendLine(line);
            sb.Append(_builder).AppendLine();
            sb.Append("DONE");

            Line = sb.ToString();
            Cloned = true;
        }

        public override void Execute()
        {
            Entity created = _factory.parsed.CreateEntity(_builder!);
            _collection.parsed.Add(created);
            Log.WriteLine($"§aCreated new §l{_collection.raw}§a:");
            Log.WriteLine(created.ToString());
        }

        public override object Clone() => new AddCommand(this);

        public override string ToHumanReadableString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").Append(_collection.raw).AppendLine("§r");
            sb.Append("representation=§e").Append(_factory.raw).AppendLine("§r");
            sb.Append(_builder.ToString(true));

            return sb.ToString();
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Type");
            writer.WriteValue(_collection.raw);
            writer.WriteEndElement();

            writer.WriteStartElement("Representation");
            writer.WriteValue(_factory.raw);
            writer.WriteEndElement();

            foreach (var field in _builder.Fields)
            {
                if(field.Value.Value == null) continue;
                writer.WriteStartElement(char.ToUpper(field.Key[0]) + field.Key[1..]);
                writer.WriteValue(field.Value.Value);
                writer.WriteEndElement();
            }
        }

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine(
                "\nwhere `§lbase|secondary§r` defines the §lrepresentation§r in which the object should be created. " +
                "After receiving the first line the program presents the user with names of all of the atomic non reference fields of this particular class. " +
                "The program waits for further instructions from the user describing the values of the fields of the object that is supposed to be created with the §ladd§r command. " +
                "The format for each line is as follows:");
            Log.WriteLine("\n\t§l<name_of_field>=<value>");
            Log.WriteLine(
                "\nA line like that means that the value of the field `§lname_of_field§r` for the newly created object will be equal to `§lvalue§r`. " +
                "The user can enter however many lines they want in such a format (even repeating the fields that they have already defined -- in this case the previous value is overridden) describing the object until using one of the following commands:");
            Log.WriteLine("\n\t§lDONE§r or §lEXIT");
            Log.WriteLine("\nAfter receiving the §lDONE§r command the creation process finishes and the program adds a new object described by the user to the collection. " +
                          "After receiving the §lEXIT§r command the creation process also finishes but no new object is created and nothing is added to the collection. " +
                          "The data provided by the user is also discarded.");
        }
    }
}
