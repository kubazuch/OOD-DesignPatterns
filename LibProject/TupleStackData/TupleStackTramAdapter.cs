using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.TupleStackData
{
    public class TupleStackTramAdapter : TupleStackVehicleAdapter, ITram
    {
        private TupleStackTram adaptee;

        public override int Id => adaptee.TupleRepr.Item1;

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

            TupleStackRepresentation.TRAMS.Add(Id, this);
            TupleStackRepresentation.VEHICLES.Add(Id, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Id).Append(", ");
            builder.Append(CarsNumber).Append(", ");
            builder.Append(Line.NumberDec);
            return builder.ToString();
        }
    }
}