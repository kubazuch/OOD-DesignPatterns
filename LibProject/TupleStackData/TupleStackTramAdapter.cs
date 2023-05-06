using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.TupleStackData
{
    public class TupleStackTramAdapter : TupleStackVehicleAdapter, ITram
    {
        private TupleStackTram adaptee;

        public override int Id => adaptee.TupleRepr.Item1;
        public override Dictionary<string, Func<object>> Fields { get; }

        public int CarsNumber
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("carsNumber"));

                return int.Parse(fromStack[i + 2]);
            }
        }

        public ILine Line
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("line"));

                return TupleStackRepresentation.LINES[int.Parse(fromStack[i + 2])];
            }
        }

        public TupleStackTramAdapter(TupleStackTram adaptee)
        {
            this.adaptee = adaptee;

            Fields = new Dictionary<string, Func<object>>()
            {
                ["id"] = () => Id,
                ["carsNumber"] = () => CarsNumber
            };

            TupleStackRepresentation.TRAMS.Add(adaptee.TupleRepr.Item1, this);
            TupleStackRepresentation.VEHICLES.Add(adaptee.TupleRepr.Item1, this);
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Tram #").Append(Id).Append(", cars: ").Append(CarsNumber).Append(", line: ").Append(Line.NumberDec);
            return builder.ToString();
        }
    }
}