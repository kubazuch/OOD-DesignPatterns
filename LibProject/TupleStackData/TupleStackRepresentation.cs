using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public abstract class TupleStackRepresentation
    {
        public abstract Tuple<int, Stack<string>> TupleRepr { get; set; }

        public string this[string name]
        {
            get
            {
                List<string> fromStack = TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals(name));

                return fromStack[i + 2];
            }
            set
            {
                List<string> fromStack = TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals(name));

                fromStack[i + 2] = value;
                fromStack.Reverse();
                
                TupleRepr = Tuple.Create(TupleRepr.Item1, new Stack<string>(fromStack));
            }
        }

        public override string ToString() => $"({TupleRepr.Item1}, {{{string.Join(", ", TupleRepr.Item2)}}})";
    }
}