using System;

namespace ConsoleProject.CLI.Exception
{
    internal class TooManyArgumentsException : ArgumentException
    {
        public TooManyArgumentsException(CommandParser cmd)
            : base($"Too many arguments for command `{cmd.FullName}`.\n\tUsage: §l{cmd}") 
        { }

        public TooManyArgumentsException(string usage)
            : base($"Too many arguments for this command.\n\tUsage: §l{usage}")
        { }
    }
}
