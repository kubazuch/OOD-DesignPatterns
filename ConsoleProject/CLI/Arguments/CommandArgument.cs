using System;
using System.Text;

namespace ConsoleProject.CLI.Arguments
{
    public interface ICommandArgument
    {
        public string Name { get; }

        public bool Required { get; }

        public object Parse(string arg);
    }

    public class CommandArgument<T> : ICommandArgument
    {
        public string Name { get; protected set; }

        public bool Required { get; protected set; }

        object ICommandArgument.Parse(string arg) => Parse(arg);

        public CommandArgument(string name, bool required = true)
        {
            Name = name;
            Required = required;
        }

        //TODO: remove
        public CommandArgument() {}

        public virtual T Parse(string arg) { return (T) Convert.ChangeType(arg, typeof(T)); }

        public override string ToString()
        {
            return new StringBuilder().Append(Required ? '<' : '[').Append(Name).Append(Required ? '>' : ']').ToString();
        }
    }
}
