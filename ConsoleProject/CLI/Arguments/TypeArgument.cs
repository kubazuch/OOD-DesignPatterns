using BTM.Collections;
using System;

namespace ConsoleProject.CLI.Arguments
{
    public class TypeArgument : CommandArgument<ICollection>
    {
        public TypeArgument(bool required = false, string name = "name_of_the_class")
        {
            this.Required = required;
            this.Name = name;
        }

        public ICollection Parse(DataManager data, string arg)
        {
            if (!data.Mapping.ContainsKey(arg))
                throw new ArgumentException($"Unknown type: `§l{arg}§r`. Possible types: §l{string.Join(", ", data.Mapping.Keys)}");
            return data.Mapping[arg];
        }
    }
}
