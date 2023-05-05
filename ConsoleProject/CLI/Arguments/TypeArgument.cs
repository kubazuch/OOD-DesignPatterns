using BTM.Collections;
using System;

namespace ConsoleProject.CLI.Arguments
{
    public class TypeArgument : CommandArgument<ICollection>
    {
        public TypeArgument(bool required = false, bool includeRaw = false, string name = "name_of_the_class")
        {
            this.Required = required;
            this.IncludeRaw = includeRaw;
            this.Name = name;
        }

        public override ICollection Parse(DataManager data, string arg)
        {
            if (!data.Mapping.ContainsKey(arg)) throw new ArgumentException($"Unknown type: {arg}. Possible types: {string.Join(", ", data.Mapping.Keys)}");
            return data.Mapping[arg];
        }
    }
}
