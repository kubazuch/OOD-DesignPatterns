using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    public class QueueCommand : Command
    {
        private static readonly EnumArgument SubcommandArg = new(new List<string> { "print", "export", "commit" }, "subcommand", true);
        private static readonly PathArgument PathArg = new(true);
        private static readonly EnumArgument FormatArg = new(new List<string> { "XML", "plaintext" }, "subcommand", false);

        private readonly CommandDispatcher _dispatcher;

        public QueueCommand(CommandDispatcher dispatcher)
            : base("queue", "Command queue commands")
        {
            _dispatcher = dispatcher;
            Line = $"queue {SubcommandArg} ...";
        }

        public override void Process(string line, List<string> context)
        {
            if (context.Count == 0)
                throw new MissingArgumentException(this, 1, SubcommandArg.Name);

            switch (SubcommandArg.Parse(context[0]))
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
            if (context.Count == 1)
                throw new MissingArgumentException($"queue export {PathArg} {FormatArg}", 1, PathArg.Name);

            var path = PathArg.Parse(context[1]);

            var format = "XML";
            if (context.Count == 3)
                format = FormatArg.Parse(context[2]);

            else if(context.Count > 3)
                throw new TooManyArgumentsException($"queue export {PathArg} {FormatArg}");

            switch (format)
            {
                case "XML":
                    SerializeXML();
                    break;
                case "plaintext":
                    SerializePlain();
                    break;
            }

            void SerializeXML()
            {
                using var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true, IndentChars = "    " });
                var serializer = new XmlSerializer(typeof(CommandQueue));
                serializer.Serialize(writer, _dispatcher.CommandQueue);
                Log.WriteLine($"§aCommand queue exported to `§l{path}§a` as XML");
            }

            void SerializePlain()
            {
                using var writer = File.CreateText(path);
                foreach (var cmd in _dispatcher.CommandQueue)
                {
                    Log.WriteLine(writer, cmd.ToString());
                    Log.WriteLine(writer);
                }
                Log.WriteLine($"§aCommand queue exported to `§l{path}§a` as plaintext");
            }
        }

        private void ProcessCommit(List<string> context)
        {
            if (context.Count > 1)
                throw new TooManyArgumentsException("queue commit");

            while (_dispatcher.CommandQueue.TryDequeue(out QueueableCommand cmd))
            {
                try
                {
                    cmd.Execute();
                }
                catch (ArgumentException ex)
                {
                    using ((TemporaryConsoleColor)ConsoleColor.DarkRed)
                    {
                        Log.WriteLine(ex.Message);
                        if (ex.InnerException != null)
                            Log.WriteLine($"Caused by: {ex.InnerException.Message}");
                    }
                }

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
                    Log.WriteLine($"\t§lqueue export {PathArg} {FormatArg}");
                    Log.WriteLine("\nSaves all commands from the queue to the file. There are supported two formats `XML`(default) and `plaintext`. The structure of XML should contain only necessary fields. The plain text format should be the same as it is in the command line – that means that pasting the content of the file to the console should add stored commands.");
                    break;
                case "commit":
                    Log.WriteLine($"§2{Name} commit§r\tExecutes all commands from the queue");
                    Log.WriteLine("\nUsage:");
                    Log.WriteLine($"\t§l{ToString()}");
                    Log.WriteLine("\nExecutes all commands stored in the queue in order of their addition. After that queue is empty.");
                    break;
                default:
                    throw new ArgumentException($"Unknown subcommand: `§l{o[0]}§r. Possible subcommands: §lprint, export, commit");
            }
        }
    }
}
