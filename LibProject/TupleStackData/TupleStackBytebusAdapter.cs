using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public sealed class TupleStackBytebusAdapter : Bytebus
    {
        private TupleStackBytebus _adaptee;

        public override int Id
        {
            get => _adaptee.TupleRepr.Item1;
            set => throw new NotImplementedException();
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

        public override string EngineClass
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("engineClass"));

                return fromStack[i + 2];
            }
            set => throw new NotImplementedException();
        }

        public TupleStackBytebusAdapter(TupleStackBytebus adaptee)
        {
            this._adaptee = adaptee;

            TupleStackRepresentation.Bytebuses.Add(Id, this);
            TupleStackRepresentation.Vehicles.Add(Id, this);
        }
    }
}