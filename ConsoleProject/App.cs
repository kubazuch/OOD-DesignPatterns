using BTM;
using BTM.Builder;
using BTM.Collections;
using BTM.TupleStackData;
using ConsoleProject.CLI;
using ConsoleProject.CLI.Arguments;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConsoleProject.CLI.Commands;

namespace ConsoleProject
{
    internal class App
    {
        public static readonly App Instance = new(DataRepresentation.Base);
        private bool _isRunning;

        private readonly CommandDispatcher _commandDispatcher;

        private App(DataRepresentation representation)
        {
            var manager = new DataManager(representation);
            _commandDispatcher = new CommandDispatcher();
            _commandDispatcher.Register(new ExitCommand(this));
            _commandDispatcher.Register(new ListCommand(manager));
            _commandDispatcher.Register(new FindCommand(manager));
            _commandDispatcher.Register(new AddCommand(manager));
            _commandDispatcher.Register(new QueueCommand(_commandDispatcher));
            _commandDispatcher.Register(new EditCommand(manager));
        }

        public void Start()
        {
            _isRunning = true;
            Log.WriteLine("Byteasar Urban Transport information system");
            Log.WriteLine("\nType \"help\" for help");
            Run();
        }
        public void Stop()
        {
            Log.WriteLine("Exiting!");
            _isRunning = false;
        }

        public void Run()
        {
            while (_isRunning)
            {
                Log.Write("§e> ");
                string input = Console.ReadLine();
                try
                {
                    _commandDispatcher.Parse(input.Trim());
                }
                catch (ArgumentException ex)
                {
                    using ((TemporaryConsoleColor) ConsoleColor.DarkRed)
                    {
                        Log.WriteLine(ex.Message);
                        if (ex.InnerException != null)
                            Log.WriteLine($"Caused by: {ex.InnerException.Message}");
                    }
                }
            }
        }
    }
}
