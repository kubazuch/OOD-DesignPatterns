using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.CLI.Arguments
{
    public class EnumArgument : CommandArgument<string>
    {
        private readonly HashSet<string> _keys;

        public EnumArgument(HashSet<string> arguments, bool required = false, string name = "enum")
        {
            this._keys = arguments;
            this.Required = required;
            this.IncludeRaw = false;
            this.Name = name;
        }

        public override string Parse(DataManager data, string arg)
        {
            if (!_keys.Contains(arg))
                throw new ArgumentException($"Invalid value: {arg}. Possible values: {string.Join(", ", _keys)}");

            return arg;
        }
    }

    public class EnumArgument<T> : CommandArgument<T>
    {
        private readonly Dictionary<string, T> _dictionary;

        public EnumArgument(Dictionary<string, T> dictionary, bool required = false, bool includeRaw = false, string name = "enum")
        {
            this._dictionary = dictionary;
            this.Required = required;
            this.IncludeRaw = includeRaw;
            this.Name = name;
        }

        public override T Parse(DataManager data, string arg)
        {
            if(!_dictionary.TryGetValue(arg, out T value))
                throw new ArgumentException($"Invalid value: {arg}. Possible values: {string.Join(", ", _dictionary.Keys)}");

            return value;
        }
    }
}
