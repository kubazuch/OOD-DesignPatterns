using System.Collections.Generic;

namespace BTM.TextData
{
    public abstract class TextRepresentation
    {
        internal static Dictionary<int, ILine> LINES = new() { [-1] = null };
        internal static Dictionary<int, IStop> STOPS = new() { [-1] = null };
        internal static Dictionary<int, IBytebus> BYTEBUSES = new() { [-1] = null };
        internal static Dictionary<int, ITram> TRAMS = new() { [-1] = null };
        internal static Dictionary<int, IVehicle> VEHICLES = new() { [-1] = null };
        internal static Dictionary<int, IDriver> DRIVERS = new() { [-1] = null };

        public abstract string TextRepr { get; }
        public override string ToString() => TextRepr;
    }
}