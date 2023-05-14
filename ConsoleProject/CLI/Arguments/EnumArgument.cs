using System;
using System.Collections.Generic;

namespace ConsoleProject.CLI.Arguments
{
    public class EnumArgument : CommandArgument<string>
    {
        private readonly HashSet<string> _keys;

        public EnumArgument(HashSet<string> arguments, string name = null, bool required = false)
        {
            this._keys = arguments;
            this.Required = required;
            this.Name = name;
        }

        public string Parse(string arg)
        {
            if (!_keys.Contains(arg))
                throw new ArgumentException($"Invalid value: `§l{arg}§r`. Possible values: §l{string.Join(", ", _keys)}");

            return arg;
        }

        public override string ToString() => string.Join('|', _keys);
    }

    public class EnumArgument<T> : CommandArgument<T>
    {
        private readonly Dictionary<string, T> _dictionary;

        public EnumArgument(Dictionary<string, T> dictionary, string name = null, bool required = false)
        {
            this._dictionary = dictionary;
            this.Required = required;
            this.Name = name;
        }

        public T Parse(string arg)
        {
            if (!_dictionary.TryGetValue(arg, out T value))
                throw new ArgumentException($"Invalid value: `§l{arg}§r`. Possible values: §l{string.Join(", ", _dictionary.Keys)}");

            return value;
        }

        public override string ToString() => string.Join('|', _dictionary.Keys);
    }
}
