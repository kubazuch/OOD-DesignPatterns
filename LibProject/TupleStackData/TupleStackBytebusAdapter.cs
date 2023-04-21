using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.TupleStackData
{
    public class TupleStackBytebusAdapter : TupleStackVehicleAdapter, IBytebus
    {
        private TupleStackBytebus adaptee;

        public override int Id => adaptee.TupleRepr.Item1;

        public List<ILine> Lines
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("lines"));
                int cnt = int.Parse(fromStack[i + 1]);
                return fromStack.GetRange(i + 2, cnt).Select(id => TupleStackRepresentation.LINES[int.Parse(id)]).ToList();
            }
        }

        public string EngineClass
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("engineClass"));

                return fromStack[i + 2];
            }
        }

        public TupleStackBytebusAdapter(TupleStackBytebus adaptee)
        {
            this.adaptee = adaptee;

            TupleStackRepresentation.BYTEBUSES.Add(Id, this);
            TupleStackRepresentation.VEHICLES.Add(Id, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Id).Append(", ");
            builder.Append('[').AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("], ");
            builder.Append(EngineClass);
            return builder.ToString();
        }
    }
}