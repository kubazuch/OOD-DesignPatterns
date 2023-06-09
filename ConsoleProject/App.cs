﻿using ConsoleProject.CLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BTM;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Commands;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject
{
    internal class App
    {
        public static readonly Regex Assignment = new(@"^([^=]+?)\s*?=\s*?(?:([^""\s]+)|""([^""]+)"")", RegexOptions.Compiled);

        public static readonly App Instance = new(DataRepresentation.TupleStack);
        private bool _isRunning;

        private readonly CommandDispatcher _commandDispatcher;
        
        public DataManager DataManager { get; }

        private App(DataRepresentation representation)
        {
            DataManager = new DataManager(representation);
            _commandDispatcher = new CommandDispatcher();

            RegisterCommands();
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

        private void Run()
        {
            while (_isRunning)
            {
                Log.Write("§e> ");
                string input = Console.ReadLine();
                try
                {
                    _commandDispatcher.Parse(input.Trim(), Console.In, true);
                }
                catch (InvalidOperationException ex)
                {
                    Log.HandleException(ex);
                }
                catch (ArgumentException ex)
                {
                    Log.HandleException(ex);
                }

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            }
        }

        private List<object?> PredicateParser(CommandParser self, List<string> context)
        {
            var collectionArg = (TypeArgument)self.Args[0];
            var predicateArg = (PredicateArgument)self.Vararg!;

            var ret = new List<object?>();
            if (context.Count == 0)
                throw new MissingArgumentException(self, 1, collectionArg.Name);

            ret.Add(self.Args[0].Parse(context[0]));

            for (int i = 1; i < context.Count; ++i)
            {
                ret.Add(predicateArg.Parse(context[0], context[i]));
            }

            return ret;
        }

        private void RegisterCommands()
        {
            _commandDispatcher.Register(CommandParser.New("exit", "Closes the application")
                .Calls((_, _, _) => Stop()));

            _commandDispatcher.Register(CommandParser.New("list", "Prints all objects of a particular type")
                .WithUsageDetails("Prints to the console all of the objects of class given by `name_of_the_class`, where printing an object means listing all of its non reference fields.")
                .WithArg(new TypeArgument(DataManager, true))
#if !HISTORY
                .Calls((args, _, _) => new ListCommand((NamedCollection)args[0]!)));
#else
                .Calls((args, _, _) => new ListCommand((NamedCollection)args[0]!).Execute()));
#endif

            _commandDispatcher.Register(CommandParser.New("find", "Prints objects matching certain conditions")
                .WithUsageDetails("where requirements (space separated list of requirements) specify acceptable values of atomic non reference fields. They follow format:\n\n\t*<name_of_field>=|<|><value>*\n" +
                                  "\nwhere `=|<|>` means any strong comparison operator. For numerical fields natural comparison will be used. Strings will use a lexicographic order. For other types only `=` is allowed. " +
                                  "If a value were to contain spaces it should be placed inside quotation marks.")
                .WithArg(new TypeArgument(DataManager, true))
                .WithVararg(new PredicateArgument(true))
                .WithParser(PredicateParser)
#if !HISTORY
                .Calls((args, _, _) => new FindCommand((NamedCollection)args[0]!,
                    args.Skip(1).Select(x => (EntityPredicate)x!).ToList())));
#else
                .Calls((args, _, _) => new FindCommand((NamedCollection)args[0]!,
                    args.Skip(1).Select(x => (EntityPredicate)x!).ToList()).Execute()));
#endif

            _commandDispatcher.Register(CommandParser.New("add", "Adds a new object of a particular type")
                .WithUsageDetails("\nwhere `base|secondary` defines the *representation* in which the object should be created. After receiving the first line the program presents the user with names of all " +
                                  "of the atomic non reference fields of this particular class. The program waits for further instructions from the user describing the values of the fields of the object that is " +
                                  "supposed to be created with the *add* command. The format for each line is as follows: \n\n\t*<name_of_field>=<value>*\n\nA line like that means that the value of the field " +
                                  "`name_of_field` for the newly created object will be equal to `value`. The user can enter however many lines they want in such a format (even repeating the fields that they " +
                                  "have already defined -- in this case the previous value is overridden) describing the object until using one of the following commands:\n\n\t*DONE* or *EXIT*\n\nAfter receiving " +
                                  "the *DONE* command the creation process finishes and the program adds a new object described by the user to the collection. After receiving the *EXIT* command the creation " +
                                  "process also finishes but no new object is created and nothing is added to the collection. The data provided by the user is also discarded.")
                .WithArg(new TypeArgument(DataManager, true))
                .WithArg(new EnumArgument<AbstractFactory>(AbstractFactory.Mapping, true, "representation"))
                .Calls(AddCommand.AddCall));

#if !HISTORY
            _commandDispatcher.Register(CommandParser.New("queue", "Command queue commands")
                .WithSubcommand(CommandParser.New("print", "Prints all commands currently stored in the queue")
                    .WithUsageDetails("Prints the name of each command stored in the queue along with all of its parameters in a human-readable form.")
                    .Calls((_, _, _) => QueueCommand.PrintCall(_commandDispatcher)))
                .WithSubcommand(CommandParser.New("export", "Exports all commands currently stored in the queue to the specified file")
                    .WithUsageDetails("Saves all commands from the queue to the file. There are supported two formats `XML` (default) and `plaintext`. The structure of XML should contain only necessary fields. The plain text format should be the same as it is in the command line – that means that pasting the content of the file to the console should add stored commands.")
                    .WithArg(new PathArgument(true))
                    .WithArg(new EnumArgument(new List<string> { "XML", "plaintext" }, false, "format"))
                    .Calls((args, _, _) => QueueCommand.ExportCall(_commandDispatcher, args)))
                .WithSubcommand(CommandParser.New("commit", "Executes all commands from the queue")
                    .WithUsageDetails("Executes all commands stored in the queue in order of their addition. After that queue is empty.")
                    .Calls((_, _, _) => QueueCommand.CommitCall(_commandDispatcher)))
                .WithSubcommand(CommandParser.New("dismiss", "Clears command queue")
                    .WithUsageDetails("This command clears all commands which are currently stored in the queue.")
                    .Calls((_, _, _) => QueueCommand.DismissCall(_commandDispatcher)))
                .WithSubcommand(CommandParser.New("load", "Loads commands to the end of the queue from the given file")
                    .WithUsageDetails("This command loads exported commands saved in a given file to the end of the queue. The loaded command should be in the same order as they were during exporting. Both file formats are supported: XML and plain - text.")
                    .WithArg(new PathArgument(true, mustExist: true))
                    .Calls((args, _, _) => QueueCommand.ImportCall(_commandDispatcher, args))));
#else
            _commandDispatcher.Register(CommandParser.New("export","Exports command history")
                .WithUsageDetails("Saves all commands from the history to the file. There are supported two formats `XML`(default) and `plaintext`. The structure of XML should contain only necessary fields. The plain text format should be the same as it is in the command line – that means that pasting the content of the file to the console should add stored commands.")
                .WithArg(new PathArgument(true))
                .WithArg(new EnumArgument(new List<string> { "XML", "plaintext" }, false, "format"))
                .Calls((args, _, _) => QueueCommand.ExportCall(_commandDispatcher, args)));

            _commandDispatcher.Register(CommandParser.New("load", "Loads commands and executes them")
                .WithUsageDetails("This command loads exported commands saved in a given file and executes them in order of file. The loaded command should be in the same order as they were during exporting. Both file formats are supported: XML and plain - text.")
                .WithArg(new PathArgument(true, mustExist: true))
                .Calls((args, _, _) => QueueCommand.ImportCall(_commandDispatcher, args)));

            _commandDispatcher.Register(CommandParser.New("history", "Prints command history")
                .WithUsageDetails("Prints (from newest) the name of each executed command along with all of its parameters in a human-readable form.")
                .Calls((_, _, _) => QueueCommand.PrintCall(_commandDispatcher)));

            _commandDispatcher.Register(CommandParser.New("undo", "Undo last command")
                .WithUsageDetails("Reverts the changes made by the most recently executed command.")
                .Calls((_, _, _) => _commandDispatcher.Undo()));

            _commandDispatcher.Register(CommandParser.New("redo", "Redo last command")
                .WithUsageDetails("Reapplies the changes made by the most recently undone command.\n\nAttention: Execution of any undoable command after `undo` prohibits further `redo`!")
                .Calls((_, _, _) => _commandDispatcher.Redo()));
#endif
            _commandDispatcher.Register(CommandParser.New("edit", "Edits values of a given record")
                .WithUsageDetails("where requirements (space separated list of requirements) specify acceptable values of atomic non reference fields. They follow format:\n\n\t*<name_of_field>=|<|><value>*\n" +
                                  "\nwhere `=|<|>` means any strong comparison operator. For numerical fields natural comparison will be used. Strings will use a lexicographic order. For other types only `=` is allowed. " +
                                  "If a value were to contain spaces it should be placed inside quotation marks.\n\nThis command allows editing a given record if requirement conditions specify *one record uniquely*.\n" +
                                  "\nAfter receiving the first line the program presents the user with names of all of the atomic non reference fields of this particular class. The program waits for further instructions " +
                                  "from the user describing the values of the fields of the object that is supposed to be edited with the *edit* command. The format for each line is as follows:\n\n\t*<name_of_field>=<value>*\n" +
                                  "\nA line like that means that the value of the field `name_of_field` for the edited object will be equal to `value`. The user can enter however many lines they want in such a format " +
                                  "(even repeating the fields that they have already defined -- in this case the previous value is overridden) describing the object until using one of the following commands:\n\n\t*DONE* or *EXIT*\n" +
                                  "\nAfter receiving the *DONE* command the edition process finishes and the program schedules the edit into the queue. After receiving the *EXIT* command the edition process also finishes " +
                                  "but no edits are done. The data provided by the user is also discarded.")
                .WithArg(new TypeArgument(DataManager, true))
                .WithVararg(new PredicateArgument(true))
                .WithParser(PredicateParser)
                .Calls(EditCommand.EditCall));

            _commandDispatcher.Register(CommandParser.New("delete", "Removes given record from collections")
                .WithUsageDetails("This command allows deleting a given record if requirement conditions (which work the same as in the *find* and *edit* command) specify one record uniquely.")
                .WithArg(new TypeArgument(DataManager, true))
                .WithVararg(new PredicateArgument(true))
                .WithParser(PredicateParser)
                .Calls((args, _, _) => new DeleteCommand((NamedCollection)args[0]!,
                    args.Skip(1).Select(x => (EntityPredicate)x!).ToList())));
        }
    }
}
