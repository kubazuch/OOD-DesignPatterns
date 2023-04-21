using System;
using System.Collections.Generic;

namespace BTM.TupleStackData
{
    public abstract class TupleStackRepresentation
    {
        internal static Dictionary<int, ILine> LINES = new();
        internal static Dictionary<int, IStop> STOPS = new();
        internal static Dictionary<int, IBytebus> BYTEBUSES = new();
        internal static Dictionary<int, ITram> TRAMS = new();
        internal static Dictionary<int, IVehicle> VEHICLES = new();
        internal static Dictionary<int, IDriver> DRIVERS = new();
        public abstract Tuple<int, Stack<string>> TupleRepr { get; }
        public override string ToString() => $"({TupleRepr.Item1}, {{{string.Join(", ", TupleRepr.Item2)}}})";
    }
}