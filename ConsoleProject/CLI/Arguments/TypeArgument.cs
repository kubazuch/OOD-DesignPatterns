using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BTM.Collections;

namespace ConsoleProject.CLI.Arguments
{
    public class TypeArgument : CommandArgument<ICollection>
    {
        public TypeArgument(bool required = false, string name = "name_of_the_class")
        {
            this.Required = required;
            this.Name = name;
        }

        public override ICollection Parse(DataManager data, string arg)
        {
            if (!data.Mapping.ContainsKey(arg)) throw new ArgumentException($"Unknown type: {arg}. Possible types: {string.Join(", ", data.Mapping.Keys)}");
            return data.Mapping[arg];
        }
    }
}
