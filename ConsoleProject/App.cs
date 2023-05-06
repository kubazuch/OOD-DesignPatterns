﻿using BTM;
using BTM.Collections;
using ConsoleProject.CLI;
using ConsoleProject.CLI.Arguments;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BTM.Builder;
using BTM.Data;
using BTM.TupleStackData;

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
                    ICollection collection = (ICollection)args[0];
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
                            if (!((Predicate<IEntity>)args[i])((IEntity)entity))
                                return false;
                        return true;
                    }

                    Algorithms.Print(collection, Predicate);
                }));

            Regex assignment = new(@"^([^=]+)=(?:([^""\s]+)|""([^""]+)"")", RegexOptions.Compiled);
            _commandDispatcher.Register(Command.Named("add")
                .WithArg(new TypeArgument(true, true))
                .WithArg(new EnumArgument<AbstractFactory>(new Dictionary<string, AbstractFactory> { ["base"] = new BaseAbstractFactory(), ["secondary"] = new TupleStackAbstractFactory() }, true))
                .Calls(args =>
                {
                    string name = (string)args[0];
                    ICollection collection = (ICollection)args[1];
                    AbstractFactory factory = (AbstractFactory)args[2];

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
                        var match = assignment.Match(cmd);

                        if (match.Success)
                        {
                            var field = match.Groups[1].Value;
                            var val = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value;
                            Console.WriteLine($"Set {field} to {val}");
                            builder.Set(field, val);
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

                    IEntity created = factory.CreateEntity(builder);
                    collection.Add(created);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Created new {name}: ");
                    Console.ResetColor();
                    Console.WriteLine(created);
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
