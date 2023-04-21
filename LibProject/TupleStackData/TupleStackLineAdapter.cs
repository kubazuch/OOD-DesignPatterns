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

            TupleStackRepresentation.LINES.Add(NumberDec, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(NumberHex).Append(", ");
            builder.Append(NumberDec).Append(", ");
            builder.Append(CommonName).Append(", ");
            builder.Append('[').AppendJoin(", ", Stops.Select(x => x.Id)).Append(']').Append(", ");
            builder.Append('[').AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }
    }
}