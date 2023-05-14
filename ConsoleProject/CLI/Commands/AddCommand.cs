using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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

        public AddCommand(DataManager data)
            : base("add", "Adds a new object of a particular type.")
        {
            _data = data;
        }

        private AddCommand(AddCommand other)
            : base(other.Name, other.Description)
        {
            _data = other._data;
        }

        public override void Process(List<string> context)
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

                if (match.Success)
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

            IsCloned = true;
        }

        public override void Execute() => Console.WriteLine(":)");

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name).Append(' ');
            if (IsCloned)
            {
                sb.Append(_collection.raw).Append(' ').AppendLine(_factory.raw);
                sb.Append(_builder).AppendLine();
                sb.Append("DONE");
            }
            else
            {
                sb.Append(TypeArg).Append(" ").Append(FactoryArg);
            }

            return sb.ToString();
        }

        public override object Clone() => new AddCommand(this);

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine($"\nwhere requirements (space separated list of requirements) specify acceptable values of atomic non reference fields. They follow format:");
            Log.WriteLine("\n\t§l<name_of_field>=|<|><value>");
            Log.WriteLine("\nwhere `§l=|<|>§r` means any strong comparison operator. For numerical fields natural comparison will be used. Strings will use a lexicographic order. For other types only `§l=§r` is allowed. If a value were to contain spaces it should be placed inside quotation marks.");
        }
    }
}
