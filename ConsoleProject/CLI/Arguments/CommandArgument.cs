using System.Text;

namespace ConsoleProject.CLI.Arguments
{
    public interface ICommandArgument
    {
        public string Name { get; }

        public bool Required { get; }
    }

    public abstract class CommandArgument<T> : ICommandArgument
    {
        public string Name { get; protected set; }

        public bool Required { get; protected set; }

        public override string ToString()
        {
            return new StringBuilder().Append(Required ? '<' : '[').Append(Name).Append(Required ? '>' : ']').ToString();
        }
    }
}
