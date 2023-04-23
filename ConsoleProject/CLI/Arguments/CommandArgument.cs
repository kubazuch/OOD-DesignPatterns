﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.CLI.Arguments
{
    public interface ICommandArgument
    {
        public string Name { get; }
        public bool Required { get; }
        public object Parse(DataManager data, string arg);
    }

    public abstract class CommandArgument<T> : ICommandArgument
    {
        public string Name { get; protected set; }
        public bool Required { get; protected set; }

        object ICommandArgument.Parse(DataManager data, string arg) => Parse(data, arg);

        public abstract T Parse(DataManager data, string arg);

        public override string ToString()
        {
            return new StringBuilder().Append(Required ? '<' : '[').Append(Name).Append(Required ? '>' : ']').ToString();
        }
    }
}
