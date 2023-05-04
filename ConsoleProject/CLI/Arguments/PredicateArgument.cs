using System;
using System.Text.RegularExpressions;
using BTM;

namespace ConsoleProject.CLI.Arguments
{
    public class PredicateArgument : CommandArgument<Predicate<IEntity>>
    {
        private Regex regex = new (@"^([^=<>]+)([=<>])([^=<>]+)$", RegexOptions.Compiled);

        public PredicateArgument(bool required = false, string name = "requirement")
        {
            this.Required = required;
            this.Name = name;
        }

        public override Predicate<IEntity> Parse(DataManager data, string arg)
        {
            var match = regex.Match(arg);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid comparison predicate: {arg}! Expected format: <name_of_field>=|<|><value>");
            }

            var name = match.Groups[1].Value;
            var cmp = match.Groups[2].Value;
            var val = match.Groups[3].Value;

            return entity =>
            {
                object field = entity.GetValueByName(name);
                if (cmp is "<" or ">" && !IsNumeric(field) && field is not string)
                    throw new ArgumentException($"Field of type {entity.GetValueByName(name).GetType()} cannot be compared using {cmp}!");

                if (IsNumeric(field) || field is string)
                {
                    object type;
                    try
                    {
                        type = Convert.ChangeType(val, field.GetType());
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"Unable to convert {val} to type {field.GetType()}.", ex);
                    }

                    var comparable = (IComparable) field;
                    
                    return cmp switch
                    {
                        "<" => comparable.CompareTo(type) < 0,
                        ">" => comparable.CompareTo(type) > 0,
                        _ => comparable.CompareTo(type) == 0
                    };
                }

                return field.Equals(val);
            };
        }
        public static bool IsNumeric(object obj)
        {
            return obj is short or ushort or int or uint or long or ulong or float or double or decimal;
        }
    }
}
