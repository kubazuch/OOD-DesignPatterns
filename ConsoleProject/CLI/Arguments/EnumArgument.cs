using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleProject.CLI.Arguments
{
    public class EnumArgument : CommandArgument<string>
    {
        private readonly List<string> _keys;

        public EnumArgument(IEnumerable<string> arguments, bool required = false, string name = null) : base(name, required)
        {
            _keys = arguments.ToList();
        }

        public override string Parse(string arg)
        {
            if (!_keys.Contains(arg))
                throw new ArgumentException($"Invalid value: `{arg}`. Possible values: §l{string.Join(", ", _keys)}");
            
            return arg;
        }

        public override string ToString() => new StringBuilder().Append(Required ? '<' : '[').Append(string.Join('|', _keys)).Append(Required ? '>' : ']').ToString();
    }

    public class EnumArgument<T> : CommandArgument<T>
    {
        private readonly Dictionary<string, T> _dictionary;

        public EnumArgument(Dictionary<string, T> dictionary, bool required = false, string name = null) : base(name, required)
        {
            _dictionary = dictionary;
        }

        public override T Parse(string arg)
        {
            if (!_dictionary.TryGetValue(arg, out T value))
                throw new ArgumentException($"Invalid value: `{arg}`. Possible values: §l{string.Join(", ", _dictionary.Keys)}");

            return value;
        }
        
        public override string ToString() => new StringBuilder().Append(Required ? '<' : '[').Append(string.Join('|', _dictionary.Keys)).Append(Required ? '>' : ']').ToString();
    }
}
