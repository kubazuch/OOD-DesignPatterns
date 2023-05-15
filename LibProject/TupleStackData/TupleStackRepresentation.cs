using System;
using System.Collections.Generic;

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
        public override string ToString() => $"({TupleRepr.Item1}, {{{string.Join(", ", TupleRepr.Item2)}}})";
    }
}