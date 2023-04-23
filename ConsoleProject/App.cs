using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTM;
using BTM.Collections;
using ConsoleProject.CLI;
using ConsoleProject.CLI.Arguments;

namespace ConsoleProject
{
    internal class App
    {
        public static readonly App Instance = new(DataRepresentation.TEXT);
        private bool _isRunning;

        private readonly DataManager _manager;
        private readonly CommandDispatcher _commandDispatcher;

        private App(DataRepresentation representation)
        {
            _manager = new DataManager(representation);
            _commandDispatcher = new CommandDispatcher(_manager);

            _commandDispatcher.Register(Command.Named("exit").Calls((_, _) => Stop()));
            //_commandDispatcher.Register(Command.Named("hello").Calls((_,_) => Console.WriteLine("Hello, world!")));
            //_commandDispatcher.Register(Command.Named("time").Calls((_,_) => Console.WriteLine($"The current time is {DateTime.Now:hh:mm:ss tt}")));
            _commandDispatcher.Register(Command.Named("list")
                .WithArg(new TypeArgument(true))
                .Calls((_, args) =>
                {
                    ICollection collection = (ICollection) args[0];
                    Algorithms.ForEach(collection.GetForwardIterator(), Console.WriteLine);
                }));
            _commandDispatcher.Register(Command.Named("find")
                .WithArg(new TypeArgument(true))
                .WithVararg(new PredicateArgument(true))
                .Calls((_, args) =>
                {
                    ICollection collection = (ICollection)args[0];
                    Predicate<object> predicate = entity =>
                    {
                        for (int i = 1; i < args.Count; i++)
                            if (!((Predicate<IEntity>) args[i])((IEntity)entity))
                                return false;
                        return true;
                    };

                    Algorithms.Print(collection, predicate);
                }));
        }

        public void Start()
        {
            _isRunning = true;
            Run();
        }
        public void Stop()
        {
            Console.WriteLine("Exiting!");
            _isRunning = false;
        }

        public void Run()
        {
            while (_isRunning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("> ");
                Console.ResetColor();
                string input = Console.ReadLine();
                try
                {
                    _commandDispatcher.Parse(input);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine($"Caused by: {ex.InnerException.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}
