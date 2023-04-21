using System.Collections.Generic;

namespace BTM.TextData
{
    public abstract class TextRepresentation
    {
        internal static Dictionary<int, ILine> LINES = new();
        internal static Dictionary<int, IStop> STOPS = new();
        internal static Dictionary<int, IBytebus> BYTEBUSES = new();
        internal static Dictionary<int, ITram> TRAMS = new();
        internal static Dictionary<int, IVehicle> VEHICLES = new();
        internal static Dictionary<int, IDriver> DRIVERS = new();

        public abstract string TextRepr { get; }
        public override string ToString() => TextRepr;
    }
}