using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public class TupleStackLine : TupleStackRepresentation
    {
        public override Tuple<int, Stack<string>> TupleRepr { get; }

        public TupleStackLine(string numberHex, int numberDec, string commonName, int[] stops, int[] vehicles)
        {
            List<string> toStack = new List<string>();

            toStack.Add("numberHex");
            toStack.Add("1");
            toStack.Add(numberHex);

            toStack.Add("commonName");
            toStack.Add("1");
            toStack.Add(commonName);

            toStack.Add("stops");
            toStack.Add(stops.Length.ToString());
            toStack.AddRange(stops.Select(x => x.ToString()));

            toStack.Add("vehicles");
            toStack.Add(vehicles.Length.ToString());
            toStack.AddRange(vehicles.Select(x => x.ToString()));

            toStack.Reverse();
            TupleRepr = Tuple.Create(numberDec, new Stack<string>(toStack));
        }
    }
}