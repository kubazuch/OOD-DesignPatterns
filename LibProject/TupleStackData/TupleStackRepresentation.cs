using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public abstract class TupleStackRepresentation
    {
        internal static Dictionary<int, Line?> Lines = new() { [-1] = null };
        internal static Dictionary<int, Stop?> Stops = new() { [-1] = null };
        internal static Dictionary<int, Bytebus?> Bytebuses = new() { [-1] = null };
        internal static Dictionary<int, Tram?> Trams = new() { [-1] = null };
        internal static Dictionary<int, Vehicle?> Vehicles = new() { [-1] = null };
        internal static Dictionary<int, Driver?> Drivers = new() { [-1] = null };

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