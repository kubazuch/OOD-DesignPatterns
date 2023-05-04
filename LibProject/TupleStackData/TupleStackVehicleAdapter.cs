using System;
using System.Collections.Generic;

namespace BTM.TupleStackData
{
    public abstract class TupleStackVehicleAdapter : IVehicle
    {
        public abstract int Id { get; }
        public abstract Dictionary<string, Func<object>> Fields { get; }
    }
}