using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.TupleStackData
{
    public class TupleStackStopAdapter : IStop
    {
        private TupleStackStop adaptee;

        public int Id => adaptee.TupleRepr.Item1;

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
        public string Name
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("name"));

                return fromStack[i + 2];
            }
        }
        public string Type
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("type"));

                return fromStack[i + 2];
            }
        }

        public TupleStackStopAdapter(TupleStackStop adaptee)
        {
            this.adaptee = adaptee;

            Fields = new Dictionary<string, Func<object>>
            {
                ["id"] = () => Id,
                ["name"] = () => Name,
                ["type"] = () => Type
            };

            TupleStackRepresentation.STOPS.Add(Id, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Stop #").Append(Id).Append(", \"").Append(Name).Append("\", type: ").AppendLine(Type);
            builder.Append("\tLines: [").AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("]");
            return builder.ToString();
        }

        public Dictionary<string, Func<object>> Fields { get; }
    }
}