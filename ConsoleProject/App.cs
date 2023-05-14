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
        public static readonly App Instance = new(DataRepresentation.TupleStack);
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

            //_commandDispatcher.Register(Command.Named("add")
            //    .WithDescription("Adds a new object of a particular type.")
            //    .WithUsageDetails("where base|secondary defines the representation in which the object should be created. After receiving the first line the program presents the user with names of all of the atomic non reference fields of this particular class. The program waits for further instructions from the user describing the values of the fields of the object that is supposed to be created with the add command. The format for each line is as follows:\n\n\t<name_of_field>=<value>\n\nA line like that means that the value of the field <name_of_field> for the newly created object will be equal to <value>. The user can enter however many lines they want in such a format (even repeating the fields that they have already defined - in this case the previous value is overridden) describing the object until using one of the following commands:\n\n\tDONE or EXIT\n\nAfter receiving the DONE command the creation process finishes and the program adds a new object described by the user to the collection. After receiving the EXIT command the creation process also finishes but no new object is created and nothing is added to the collection. The data provided by the user is also discarded.")
            //    .WithArg(new TypeArgument(true, true))
            //    .WithArg(new EnumArgument<AbstractFactory>(new Dictionary<string, AbstractFactory> { ["base"] = new BaseAbstractFactory(), ["secondary"] = new TupleStackAbstractFactory() }, true))
            //    .Calls(AddCommandLogic));
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

        private void AddCommandLogic(List<(string raw, object? parsed)> args)
        {
            string name = args[0].raw;
            ICollection collection = (ICollection)args[0].parsed!;
            AbstractFactory factory = (AbstractFactory)args[1].parsed!;

            AbstractBuilder builder = AbstractBuilder.GetByType(name);

            Console.WriteLine($"Adding new {name}.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Available fields: {string.Join(", ", builder.Fields)}");

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("+ ");
                Console.ResetColor();
                var cmd = Console.ReadLine();
                var match = AddCommand.Assignment.Match(cmd);

                if (match.Success)
                {
                    var field = match.Groups[1].Value;
                    var val = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value;
                    Console.WriteLine($"Set {field} to {val}");
                    try
                    {
                        //builder.Set(field, val);
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
                else if (cmd is "DONE")
                    break;
                else if (cmd is "EXIT")
                {
                    Console.WriteLine($"Creation of {name} terminated.");
                    return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $"Unknown subcommand: {cmd}. Possible subcommands: EXIT, DONE and assigmnents of form: field=value");
                }
            } while (true);

            Entity created = factory.CreateEntity(builder);
            collection.Add(created);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Created new {name}: ");
            Console.ResetColor();
            Console.WriteLine(created);
        }
    }
}
