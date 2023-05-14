using System;
using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM.Builder
{
    public class TramBuilder : AbstractBuilder
    {
        public static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int?>("id"), new Field<int?>("carsNumber")
        }.ToDictionary(f => f.Name, f => f);

        internal int? Id;
        internal int? CarsNumber;

        public TramBuilder(bool init = true) : base("tram", AvailableFields)
        {
            AssignSettersAndGetters();

            if (!init) return;

            Id = 0;
            CarsNumber = 2;
        }

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int?>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var carsNumber = (Field<int?>)Fields["carsNumber"];
            carsNumber.SetSetter(value => CarsNumber = value);
            carsNumber.SetGetter(() => CarsNumber);
        }

        public TramBuilder SetId(int id)
        {
            this.Id = id;
            return this;
        }

        public TramBuilder SetCarsNumber(int number)
        {
            this.CarsNumber = number;
            return this;
        }
    }
}
