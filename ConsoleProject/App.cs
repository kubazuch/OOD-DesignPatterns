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

            _commandDispatcher.Register(Command.Named("exit").WithDescription("Closes the application.").Calls(_ => Stop()));
            _commandDispatcher.Register(Command.Named("list")
                .WithDescription("Prints all objects of a particular type.")
                .WithUsageDetails("The command prints to the console all of the objects of given class where printing an object\nmeans listing all of its non reference fields.")
                .WithArg(new TypeArgument(true))
                .Calls(args =>
                {
                    ICollection collection = (ICollection) args[0];
                    Algorithms.ForEach(collection.GetForwardIterator(), Console.WriteLine);
                }));
            _commandDispatcher.Register(Command.Named("find")
                .WithDescription("Prints objects matching certain conditions.")
                .WithUsageDetails("where requirements (space separated list of requirements) specify acceptable values of atomic non\nreference fields. They follow format:\n\n\t<name_of_field>=|<|><value>\n\nwhere “=|<|>” means any strong comparison operator. For numerical fields natural comparison will\nbe used. Strings will use a lexicographic order. For other types only “=” is allowed. If a value\nwere to contain spaces it should be placed inside quotation marks.")
                .WithArg(new TypeArgument(true))
                .WithVararg(new PredicateArgument(true))
                .Calls(args =>
                {
                    ICollection collection = (ICollection)args[0];

                    bool Predicate(object entity)
                    {
                        for (int i = 1; i < args.Count; i++)
                            if (!((Predicate<IEntity>) args[i])((IEntity) entity))
                                return false;
                        return true;
                    }

                    Algorithms.Print(collection, Predicate);
                }));
        }

        public void Start()
        {
            _isRunning = true;
            Console.WriteLine("Byteasar Urban Transport information system");
            Console.WriteLine("\nType \"help\" for help");
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
