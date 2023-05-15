using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public class TupleStackStop : TupleStackRepresentation
    {
        public override Tuple<int, Stack<string>> TupleRepr { get; set; }

        public TupleStackStop(int id, string name, string type, params int[] lines)
        {
            List<string> toStack = new List<string>();

            toStack.Add("name");
            toStack.Add("1");
            toStack.Add(name);

            toStack.Add("type");
            toStack.Add("1");
            toStack.Add(type);

            toStack.Add("lines");
            toStack.Add(lines.Length.ToString());
            toStack.AddRange(lines.Select(x => x.ToString()));

            toStack.Reverse();
            TupleRepr = Tuple.Create(id, new Stack<string>(toStack));
        }

    }
}