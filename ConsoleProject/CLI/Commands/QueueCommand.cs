using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTM;
using BTM.BaseData;
using BTM.TupleStackData;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    public class QueueCommand : Command
    {
        private static readonly EnumArgument FactoryArg = new(new List<string> { "print", "export", "commit" }, "subcommand", true);

        private readonly CommandDispatcher _dispatcher;

        public QueueCommand(CommandDispatcher dispatcher)
            : base("queue", "Command queue commands")
        {
            _dispatcher = dispatcher;
            Line = $"queue {FactoryArg} ...";
        }

        public override void Process(string line, List<string> context)
        {
            if (context.Count == 0)
                throw new MissingArgumentException(this, 1, FactoryArg.Name);

            switch (FactoryArg.Parse(context[0]))
            {
                case "print":
                    ProcessPrint(context);
                    break;
                case "export":
                    ProcessExport(context);
                    break;
                case "commit":
                    ProcessCommit(context);
                    break;
            }
        }

        private void ProcessPrint(List<string> context)
        {
            if (context.Count > 1)
                throw new TooManyArgumentsException("queue print");

            foreach (var cmd in _dispatcher.CommandQueue)
            {
                Log.WriteLine(cmd.ToHumanReadableString());
                Log.WriteLine();
            }
        }

        private void ProcessExport(List<string> context)
        {
            //TODO: exporting
            Log.WriteLine("export");
        }

        private void ProcessCommit(List<string> context)
        {
            if (context.Count > 1)
                throw new TooManyArgumentsException("queue commit");

            while (_dispatcher.CommandQueue.TryDequeue(out QueueableCommand cmd))
            {
                cmd.Execute();
                Log.WriteLine();
            }
        }

        public override void PrintHelp(List<string>? o)
        {
            if (o == null)
            {
                base.PrintHelp();
                return;
            }
            else if (o.Count == 0)
            {
                base.PrintHelp();
                Log.WriteLine("\nUsage:");
                Log.WriteLine($"\t§l{ToString()}");
                Log.WriteLine("\nType `§lhelp queue <subcommand>§r` for more details.");
                return;
            }

            switch (o[0])
            {
                case "print":
                    Log.WriteLine($"§2{Name} print§r\tPrints all commands currently stored in the queue");
                    Log.WriteLine("\nUsage:");
                    Log.WriteLine($"\t§lqueue print");
                    Log.WriteLine("\nPrints the name of each command stored in the queue along with all of its parameters in a human-readable form.");
                    break;
                case "export":
                    Log.WriteLine($"§2{Name} export§r\tExports all commands currently stored in the queue to the specified file");
                    Log.WriteLine("\nUsage:");
                    Log.WriteLine($"\t§lqueue export TODO"); //TODO: correct
                    Log.WriteLine("\nSaves all commands from the queue to the file. There are supported two formats `XML`(default) and `plaintext`. The structure of XML should contain only necessary fields. The plain text format should be the same as it is in the command line – that means that pasting the content of the file to the console should add stored commands.");
                    break;
                case "commit":
                    Log.WriteLine($"§2{Name} commit§r\tExecutes all commands from the queue");
                    Log.WriteLine("\nUsage:");
                    Log.WriteLine($"\t§l{ToString()}");
                    Log.WriteLine("\nExecutes all commands stored in the queue in order of their addition. After that queue is empty.");
                    break;
            }
        }
    }
}
