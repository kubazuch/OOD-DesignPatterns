using System.Collections.Generic;

namespace BTM.TextData
{
    public abstract class TextRepresentation
    {
        internal static Dictionary<int, Line?> Lines = new() { [-1] = null };
        internal static Dictionary<int, Stop?> Stops = new() { [-1] = null };
        internal static Dictionary<int, Bytebus?> Bytebuses = new() { [-1] = null };
        internal static Dictionary<int, Tram?> Trams = new() { [-1] = null };
        internal static Dictionary<int, Vehicle?> Vehicles = new() { [-1] = null };
        internal static Dictionary<int, Driver?> Drivers = new() { [-1] = null };

        public abstract string TextRepr { get; }
        public override string ToString() => TextRepr;
    }
}