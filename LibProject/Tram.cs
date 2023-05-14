using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Tram : Vehicle
    {
        public new static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int>("id"), new Field<int>("carsNumber")
        }.ToDictionary(f => f.Name, f => f);

        public abstract int CarsNumber { get; set; }
        public abstract Line? Line { get; }

        protected Tram()
            : base(AvailableFields)
        {
            AssignSettersAndGetters();
        }

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var carsNumber = (Field<int>)Fields["carsNumber"];
            carsNumber.SetSetter(value => CarsNumber = value);
            carsNumber.SetGetter(() => CarsNumber);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Tram #").Append(Id).Append(", cars: ").Append(CarsNumber).Append(", line: ").Append(Line?.NumberDec.ToString() ?? "none");
            return builder.ToString();
        }
    }
}