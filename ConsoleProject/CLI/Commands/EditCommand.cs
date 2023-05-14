
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BTM;
using BTM.Builder;
using BTM.Collections;
using BTM.Refraction;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    public class EditCommand : QueueableCommand
    {
        private static readonly TypeArgument TypeArg = new(true);
        private static readonly PredicateArgument PredicateArg = new(true);
        public static readonly Regex Assignment = new(@"^([^=]+?)\s*?=\s*?(?:([^""\s]+)|""([^""]+)"")", RegexOptions.Compiled);

        private readonly DataManager _data;

        private (string raw, ICollection parsed) _collection;
        private (List<string> raw, Predicate<object> parsed) _predicate;
        private AbstractBuilder? _builder;

        public EditCommand() : base("", "") => throw new NotImplementedException();

        public EditCommand(DataManager data)
            : base("edit", "Edits values of a given record")
        {
            _data = data;

            var sb = new StringBuilder();
            sb.Append(Name).Append(' ').Append(TypeArg).Append(" [").Append(PredicateArg).Append(" ...]");
            Line = sb.ToString();
        }

        private EditCommand(EditCommand other)
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
            _builder = AbstractBuilder.GetByType(_collection.raw, false);

            List<Predicate<Entity>> predicates = new List<Predicate<Entity>>();
            for (int i = 1; i < context.Count; ++i)
            {
                predicates.Add(PredicateArg.Parse(context[0], context[i]));
            }

            _predicate = (context.Skip(1).ToList(), entity => predicates.All(predicate => predicate((Entity)entity)));

            Log.WriteLine($"Editing §l{_collection.raw}§r");
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
                        ((IRefractive)_builder).SetValueByName(field, val);
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
                    Log.WriteLine($"§eEdition of {_collection.raw} terminated.");
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
            Entity? entity = null;
            int count = 0;
            var iterator = _collection.parsed.GetForwardIterator();
            while (iterator.MoveNext())
            {
                if (!_predicate.parsed(iterator.Current)) continue;
                count++;
                entity ??= (Entity) iterator.Current;
            }
            
            if (count != 1)
                throw new ArgumentException($"Predicate `§l{string.Join("§r and §l", _predicate.raw.Skip(1))}§r` should specify one record uniquely, found: §l{count}");

            foreach (var kvp in _builder.Fields)
            {
                var field = kvp.Value;
                if(field.Value == null) continue;

                ((IRefractive) entity!).SetValueByName(field.Name, field.Value);
            }

            Log.WriteLine($"§aEdited §l{_collection.raw}§a:");
            Log.WriteLine(entity.ToString());
        }

        public override string ToHumanReadableString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").AppendLine(_collection.raw);
            sb.Append("§rpredicate=§e").Append(string.Join("§r and §e", _predicate.raw.Skip(1))).Append("§r");
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

            foreach (var predicate in _predicate.raw)
            {
                writer.WriteStartElement("Predicate");
                writer.WriteValue(predicate);
                writer.WriteEndElement();
            }

            foreach (var field in _builder.Fields)
            {
                if (field.Value.Value == null) continue;
                writer.WriteStartElement(char.ToUpper(field.Key[0]) + field.Key[1..]);
                writer.WriteValue(field.Value.Value);
                writer.WriteEndElement();
            }
        }

        public override object Clone() => new EditCommand(this);

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine($"\nwhere requirements (space separated list of requirements) specify acceptable values of atomic non reference fields. They follow format:");
            Log.WriteLine("\n\t§l<name_of_field>=|<|><value>");
            Log.WriteLine("\nwhere `§l=|<|>§r` means any strong comparison operator. For numerical fields natural comparison will be used. Strings will use a lexicographic order. For other types only `§l=§r` is allowed. If a value were to contain spaces it should be placed inside quotation marks.");
            Log.WriteLine("\nThis command allows editing a given record if requirement conditions specify §lone record uniquely§r.");
            Log.WriteLine("\nAfter receiving the first line the program presents the user with names of all of the atomic non reference fields of this particular class. " +
                          "The program waits for further instructions from the user describing the values of the fields of the object that is supposed to be edited with the §ledit§r command. " +
                          "The format for each line is as follows:");
            Log.WriteLine("\n\t§l<name_of_field>=<value>");
            Log.WriteLine(
                "\nA line like that means that the value of the field `§lname_of_field§r` for the edited object will be equal to `§lvalue§r`. " +
                "The user can enter however many lines they want in such a format (even repeating the fields that they have already defined -- in this case the previous value is overridden) describing the object until using one of the following commands:");
            Log.WriteLine("\n\t§lDONE§r or §lEXIT");
            Log.WriteLine("\nAfter receiving the §lDONE§r command the edition process finishes and the program schedules the edit into the queue. " +
                          "After receiving the §lEXIT§r command the edition process also finishes but no edits are done. " +
                          "The data provided by the user is also discarded.");
        }
    }
}
