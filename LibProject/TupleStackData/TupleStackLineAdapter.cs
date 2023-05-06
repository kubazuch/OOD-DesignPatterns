using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.TupleStackData
{
    public class TupleStackLineAdapter : ILine
    {
        private TupleStackLine adaptee;

        public string NumberHex
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("numberHex"));

                return fromStack[i + 2];
            }
        }

        public int NumberDec => adaptee.TupleRepr.Item1;

        public string CommonName
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("commonName"));

                return fromStack[i + 2];
            }
        }

        public List<IStop> Stops
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("stops"));
                int cnt = int.Parse(fromStack[i + 1]);
                return fromStack.GetRange(i + 2, cnt).Select(id => TupleStackRepresentation.STOPS[int.Parse(id)]).ToList();
            }
        }

        public List<IVehicle> Vehicles
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("vehicles"));
                int cnt = int.Parse(fromStack[i + 1]);
                return fromStack.GetRange(i + 2, cnt).Select(id => TupleStackRepresentation.VEHICLES[int.Parse(id)]).ToList();
            }
        }

        public TupleStackLineAdapter(TupleStackLine adaptee)
        {
            this.adaptee = adaptee;

            Fields = new Dictionary<string, Func<object>>
            {
                ["numberHex"] = () => NumberHex,
                ["numberDec"] = () => NumberDec,
                ["commonName"] = () => CommonName
            };

            TupleStackRepresentation.LINES.Add(NumberDec, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Line 0x").Append(NumberHex).Append(" (").Append(NumberDec).Append("), \"").Append(CommonName).AppendLine("\"");
            builder.Append("\tStops: [").AppendJoin(", ", Stops.Select(x => x.Id)).AppendLine("]");
            builder.Append("\tVehicles: [").AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }

        public Dictionary<string, Func<object>> Fields { get; }
    }
}