using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public sealed class TupleStackStopAdapter : Stop
    {
        private readonly TupleStackStop _adaptee;

        public override int Id
        {
            get => _adaptee.TupleRepr.Item1;
            set => _adaptee.TupleRepr = Tuple.Create(value, _adaptee.TupleRepr.Item2);
        }

        public override List<Line> Lines
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("lines"));
                int cnt = int.Parse(fromStack[i + 1]);
                return fromStack.GetRange(i + 2, cnt).Select(id => TupleStackRepresentation.Lines[int.Parse(id)]).ToList();
            }
        }
        public override string Name
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("name"));

                return fromStack[i + 2];
            }
            set
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("name"));

                fromStack[i + 2] = value;
                fromStack.Reverse();

                _adaptee.TupleRepr = Tuple.Create(_adaptee.TupleRepr.Item1, new Stack<string>(fromStack));
            }
        }

        public override string Type
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("type"));

                return fromStack[i + 2];
            }
            set
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("type"));

                fromStack[i + 2] = value;
                fromStack.Reverse();

                _adaptee.TupleRepr = Tuple.Create(_adaptee.TupleRepr.Item1, new Stack<string>(fromStack));
            }
        }

        public TupleStackStopAdapter(TupleStackStop adaptee)
        {
            this._adaptee = adaptee;

            TupleStackRepresentation.Stops.Add(Id, this);
        }
    }
}