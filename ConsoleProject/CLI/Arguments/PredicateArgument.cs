using BTM;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BTM.Refraction;

namespace ConsoleProject.CLI.Arguments
{
    public class PredicateArgument : CommandArgument<Predicate<Entity>>
    {
        public static readonly HashSet<Type> NumericTypes = new()
        {
            typeof(decimal), typeof(int),  typeof(uint),  typeof(long),  typeof(ulong), typeof(short), 
            typeof(ushort),  typeof(byte), typeof(sbyte), typeof(float), typeof(double)
        };

        private readonly Regex _regex = new(@"^([^=<>]+)([=<>])([^=<>]+)$", RegexOptions.Compiled);

        public PredicateArgument(bool required = false, string name = "requirement")
        {
            this.Required = required;
            this.Name = name;
        }

        public Predicate<Entity> Parse(string type, string arg)
        {
            var match = _regex.Match(arg);
            if (!match.Success)
                throw new ArgumentException($"Invalid comparison predicate: `§l{arg}§r`. Expected format: §l<name_of_field>=|<|><value>");

            var name = match.Groups[1].Value;
            var cmp = match.Groups[2].Value;
            var val = match.Groups[3].Value;

            if (!Entity.AvailableFields[type].TryGetValue(name, out var field))
                throw new ArgumentException($"Unknown field: `§l{name}§r`. Possible names: §l{string.Join(", ", Entity.AvailableFields[type].Keys)}");

            if (!NumericTypes.Contains(field.FieldType) && field.FieldType != typeof(string))
            {
                if (cmp is "<" or ">")
                {
                    throw new ArgumentException(
                        $"Field of type `§l{field.FieldType}§r` cannot be compared using §l{cmp}§r!");
                }

                return entity =>
                {
                    var fieldValue = (IComparable?) ((IRefractive) entity).GetValueByName(name);
                    return fieldValue != null && fieldValue.Equals(val);
                };
            }

            object converted;
            try
            {
                converted = Convert.ChangeType(val, field.FieldType);
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException($"Unable to convert `§l{val}§r` to type §l{field.FieldType}§r.", ex);
            }
                
            return entity =>
            {
                var fieldValue = (IComparable?) ((IRefractive) entity).GetValueByName(name);
                if (fieldValue == null) return false;

                return cmp switch
                {
                    "<" => fieldValue.CompareTo(converted) < 0,
                    ">" => fieldValue.CompareTo(converted) > 0,
                    _ => fieldValue.CompareTo(converted) == 0
                };
            };
        }
    }
}
