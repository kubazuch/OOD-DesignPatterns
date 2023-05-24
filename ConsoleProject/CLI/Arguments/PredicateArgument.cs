using BTM;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using BTM.Refraction;

namespace ConsoleProject.CLI.Arguments
{
    public class PredicateArgument : CommandArgument<EntityPredicate>
    {
        private readonly Regex _regex = new(@"^([^=<>]+)([=<>])([^=<>]+)$", RegexOptions.Compiled);

        public PredicateArgument(bool required = false, string name = "requirement") : base(name, required)
        {
        }

        public override EntityPredicate Parse(string arg) => throw new NotImplementedException("Please use custom parser!");

        public EntityPredicate Parse(string type, string arg)
        {
            var match = _regex.Match(arg);
            if (!match.Success)
                throw new ArgumentException($"Invalid comparison predicate: `{arg}`. Expected format: *<name_of_field>=|<|><value>*");

            var name = match.Groups[1].Value;
            var cmp = match.Groups[2].Value;
            var val = match.Groups[3].Value;

            return new EntityPredicate(type, name, cmp, val);
        }
    }

    public class EntityPredicate : IXmlSerializable
    {
        public static readonly HashSet<Type> NumericTypes = new()
        {
            typeof(decimal),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            typeof(byte),
            typeof(sbyte),
            typeof(float),
            typeof(double)
        };

        public string Name { get; }
        public string Operator { get; }
        public object Value { get; }
        public Predicate<Entity> Predicate { get; }

        public EntityPredicate() => throw new NotImplementedException();

        public EntityPredicate(string type, string name, string cmp, string value)
        {
            Name = name;
            Operator = cmp;
            Value = value;

            if (!Entity.AvailableFields[type].TryGetValue(name, out var field))
                throw new ArgumentException($"Unknown field: `{name}`. Possible names: *{string.Join(", ", Entity.AvailableFields[type].Keys)}*");

            if (!NumericTypes.Contains(field.FieldType) && field.FieldType != typeof(string))
            {
                if (cmp is "<" or ">")
                {
                    throw new ArgumentException(
                        $"Field of type `{field.FieldType}` cannot be compared using *{cmp}*!");
                }

                Predicate = entity =>
                {
                    var fieldValue = ((IRefractive)entity)[name];
                    return fieldValue != null && fieldValue.Equals(value);
                };

                return;
            }

            object converted;
            try
            {
                converted = Convert.ChangeType(value, field.FieldType);
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException($"Unable to convert `{value}` to type *{field.FieldType}*.", ex);
            }

            Predicate = entity =>
            {
                var fieldValue = (IComparable?)((IRefractive)entity)[name];
                if (fieldValue == null) return false;

                return cmp switch
                {
                    "<" => fieldValue.CompareTo(converted) < 0,
                    ">" => fieldValue.CompareTo(converted) > 0,
                    _ => fieldValue.CompareTo(converted) == 0
                };
            };
        }

        public override string ToString() => $"{Name}{Operator}{Value.ToString()!.Enquote()}";

        public XmlSchema? GetSchema() => null;

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("field", Name);
            writer.WriteAttributeString("comparison", Operator);
            writer.WriteValue(Value);
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
