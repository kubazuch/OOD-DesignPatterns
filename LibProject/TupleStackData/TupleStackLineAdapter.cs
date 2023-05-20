using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public sealed class TupleStackLineAdapter : Line
    {
        private readonly TupleStackLine _adaptee;

        public override string NumberHex
        {
            get => _adaptee["numberHex"];
            set => _adaptee["numberHex"] = value;
        }

        public override int NumberDec
        {
            get => _adaptee.TupleRepr.Item1;
            set => _adaptee.TupleRepr = Tuple.Create(value, _adaptee.TupleRepr.Item2);
        }

        public override string CommonName
        {
            get => _adaptee["commonName"];
            set => _adaptee["commonName"] = value;
        }

        public override List<Stop> Stops
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("stops"));
                int cnt = int.Parse(fromStack[i + 1]);
                return fromStack.GetRange(i + 2, cnt).Select(id => TupleStackRepresentation.Stops[int.Parse(id)]).ToList();
            }
        }

        public override List<Vehicle> Vehicles
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("vehicles"));
                int cnt = int.Parse(fromStack[i + 1]);
                return fromStack.GetRange(i + 2, cnt).Select(id => TupleStackRepresentation.Vehicles[int.Parse(id)]).ToList();
            }
        }

        public TupleStackLineAdapter(TupleStackLine adaptee)
        {
            this._adaptee = adaptee;

            TupleStackRepresentation.Lines.Add(NumberDec, this);
        }
    }
}