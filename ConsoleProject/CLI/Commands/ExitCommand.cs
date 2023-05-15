﻿using System.Collections.Generic;
using System.IO;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    internal class ExitCommand : Command
    {
        private readonly App _app;
        public ExitCommand(App app) 
            : base("exit", "Closes the application")
        {
            _app = app;
            Line = "exit";
        }

        public override void Process(List<string> raw, List<string> context, TextReader source, bool silent = false)
        {
            if (context.Count > 0)
                throw new TooManyArgumentsException(this);

            _app.Stop();
        }

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
        }
    }
}
