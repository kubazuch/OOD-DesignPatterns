using System;
using System.Collections.Generic;

namespace BTM.TextData
{
    public abstract class TextVehicleAdapter : IVehicle
    {
        public abstract int Id { get; }
        public abstract Dictionary<string, Func<object>> Fields { get; }
    }
}