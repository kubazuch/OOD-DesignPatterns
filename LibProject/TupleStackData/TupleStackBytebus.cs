using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public class TupleStackBytebus : TupleStackVehicle
    {
        public override Tuple<int, Stack<string>> TupleRepr { get; }

        public TupleStackBytebus(int id, string engineClass, params int[] lines)
        {
            List<string> toStack = new List<string>();

            toStack.Add("engineClass");
            toStack.Add("1");
            toStack.Add(engineClass);

            toStack.Add("lines");
            toStack.Add(lines.Length.ToString());
            toStack.AddRange(lines.Select(x => x.ToString()));

            toStack.Reverse();
            TupleRepr = Tuple.Create(id, new Stack<string>(toStack));
        }
    }
}