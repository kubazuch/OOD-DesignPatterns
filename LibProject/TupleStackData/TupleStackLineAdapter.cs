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
            set
            {
                ChangeId(value);
                _adaptee.TupleRepr = Tuple.Create(value, _adaptee.TupleRepr.Item2);
            }
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
                try
                {
                    return fromStack.GetRange(i + 2, cnt).Select(id => Vault.Stops[int.Parse(id)]).ToList();
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ArgumentException("Reference to nonexistent Stop", ex);
                }
            }
        }

        public override List<Vehicle> Vehicles
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("vehicles"));
                int cnt = int.Parse(fromStack[i + 1]);
                try
                {
                    return fromStack.GetRange(i + 2, cnt).Select(id => Vault.Vehicles[int.Parse(id)]).ToList();
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ArgumentException("Reference to nonexistent Vehicle", ex);
                }
            }
        }

        public TupleStackLineAdapter(TupleStackLine adaptee)
        {
            this._adaptee = adaptee;
        }

        public override void OnStopDeleted(Stop stop)
        {
        }

        public override void OnVehicleDeleted(Vehicle vehicle)
        {
        }
    }
}