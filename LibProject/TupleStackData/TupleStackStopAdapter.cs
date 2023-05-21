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
            set
            {
                ChangeId(value);
                _adaptee.TupleRepr = Tuple.Create(value, _adaptee.TupleRepr.Item2);
            }
        }

        public override List<Line> Lines
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("lines"));
                int cnt = int.Parse(fromStack[i + 1]);
                try
                {
                    return fromStack.GetRange(i + 2, cnt).Select(id => Vault.Lines[int.Parse(id)]).ToList();
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ArgumentException("Reference to nonexistent Line", ex);
                }
            }
        }
        public override string Name
        {
            get => _adaptee["name"];
            set => _adaptee["name"] = value;
        }

        public override string Type
        {
            get => _adaptee["type"];
            set => _adaptee["type"] = value;
        }

        public TupleStackStopAdapter(TupleStackStop adaptee)
        {
            this._adaptee = adaptee;
        }

        public override void OnLineDeleted(Line line)
        {
        }
    }
}