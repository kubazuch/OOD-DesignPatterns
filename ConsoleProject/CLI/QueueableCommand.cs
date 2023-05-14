﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.CLI
{
    public abstract class QueueableCommand : Command, ICloneable
    {
        protected bool Cloned = false;

        protected QueueableCommand(string name, string description) 
            : base(name, description)
        {
        }

        public abstract object Clone();

        public bool IsCloned() => Cloned;

        public abstract void Execute();

        public abstract override string ToHumanReadableString();

        public override string ToString() => Line;
    }
}
