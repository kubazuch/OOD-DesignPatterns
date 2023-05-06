using System;
using System.Collections.Generic;

namespace BTM.Builder
{
    public class TramBuilder : AbstractBuilder
    {
        internal int _id;
        internal int _carsNumber;

        public TramBuilder() : base("tram")
        {
            Setters = new Dictionary<string, Func<string, AbstractBuilder>>
            {
                ["id"] = v => SetId(int.Parse(v)),
                ["carsNumber"] = v => SetCarsNumber(int.Parse(v))
            };
        }

        public TramBuilder SetId(int id)
        {
            this._id = id;
            return this;
        }

        public TramBuilder SetCarsNumber(int number)
        {
            this._carsNumber = number;
            return this;
        }
    }
}
