﻿using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using BTM.Collections;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    public class ListCommand : QueueableCommand
    {
        private static readonly TypeArgument TypeArg = new (true);

        private readonly DataManager _data;

        private (string raw, ICollection parsed) _collection;

        public ListCommand(DataManager data) 
            : base("list", "Prints objects matching certain conditions")
        {
            _data = data;
            Line = $"{Name} {TypeArg}";
        }

        private ListCommand(ListCommand other)
            : base(other.Name, other.Description)
        {
            _data = other._data;

            Line = other.Line;
        }

        public override void Process(string line, List<string> context)
        {
            if (context.Count == 0)
                throw new MissingArgumentException(this, 1, TypeArg.Name);

            _collection = (context[0], TypeArg.Parse(_data, context[0]));

            if (context.Count > 1)
                throw new TooManyArgumentsException(this);

            Line = line;
            IsCloned = true;
        }

        public override void Execute() => Algorithms.ForEach(_collection.parsed.GetForwardIterator(), Console.WriteLine);

        public override object Clone() => new ListCommand(this);

        public override string ToHumanReadableString()
        {
            var sb = new StringBuilder();
            sb.Append("§6").Append(Name).Append("§r").Append(':').AppendLine();
            sb.Append("type=§e").Append(_collection.raw).Append("§r");

            return sb.ToString();
        }

        public override void PrintHelp(List<string>? o)
        {
            base.PrintHelp();
            if (o == null) return;

            Log.WriteLine("\nUsage:");
            Log.WriteLine($"\t§l{ToString()}");
            Log.WriteLine($"\nPrints to the console all of the objects of class given by `§l{TypeArg.Name}§r`, where printing an object means listing all of its non reference fields.");
        }
    }
}