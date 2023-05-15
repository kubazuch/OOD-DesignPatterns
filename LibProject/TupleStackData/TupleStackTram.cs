using System;
using System.Collections.Generic;

namespace BTM.TupleStackData
{
    public class TupleStackTram : TupleStackVehicle
    {
        public override Tuple<int, Stack<string>> TupleRepr { get; set; }

        public TupleStackTram(int id, int carsNumber, int line)
        {
            List<string> toStack = new List<string>();

            toStack.Add("carsNumber");
            toStack.Add("1");
            toStack.Add(carsNumber.ToString());

            toStack.Add("line");
            toStack.Add("1");
            toStack.Add(line.ToString());

            toStack.Reverse();
            TupleRepr = Tuple.Create(id, new Stack<string>(toStack));
        }
    }
}