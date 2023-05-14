using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.CLI.Exception
{
    internal class TooManyArgumentsException : ArgumentException
    {
        public TooManyArgumentsException(Command cmd)
            : base($"Too many arguments for this command.\n\tUsage: §l{cmd}") 
        { }
    }
}
