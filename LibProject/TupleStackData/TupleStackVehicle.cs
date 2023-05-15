using System;
using System.Collections.Generic;

namespace BTM.TupleStackData
{
    public abstract class TupleStackVehicle : TupleStackRepresentation
    {
        public abstract override Tuple<int, Stack<string>> TupleRepr { get; set; }
    }
}