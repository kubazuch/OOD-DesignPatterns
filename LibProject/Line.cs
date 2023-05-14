using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Line : Entity
    {
        public new static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<string>("numberHex"), new Field<int>("numberDec"), new Field<string>("commonName")
        }.ToDictionary(f => f.Name, f => f);

        public abstract string NumberHex { get; set; }
        public abstract int NumberDec { get; set; }
        public abstract string CommonName { get; set; }
        public abstract List<Stop> Stops { get; }
        public abstract List<Vehicle> Vehicles { get; }

        public Line()
            : base(AvailableFields)
        {
            AssignSettersAndGetters();
        }

        public sealed override void AssignSettersAndGetters()
        {
            var numberHex = (Field<string>)Fields["numberHex"];
            numberHex.SetSetter(value => NumberHex = value);
            numberHex.SetGetter(() => NumberHex);

            var numberDec = (Field<int>)Fields["numberDec"];
            numberDec.SetSetter(value => NumberDec = value);
            numberDec.SetGetter(() => NumberDec);

            var commonName = (Field<string>)Fields["commonName"];
            commonName.SetSetter(value => CommonName = value);
            commonName.SetGetter(() => CommonName);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Line 0x").Append(NumberHex).Append(" (").Append(NumberDec).Append("), \"").Append(CommonName).AppendLine("\"");
            builder.Append("\tStops: [").AppendJoin(", ", Stops.Select(x => x.Id)).AppendLine("]");
            builder.Append("\tVehicles: [").AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }

    }
}
