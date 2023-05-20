using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public sealed class TupleStackDriverAdapter : Driver
    {
        private readonly TupleStackDriver _adaptee;

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

        public override string Name
        {
            get => _adaptee["name"];
            set => _adaptee["name"] = value;
        }

        public override string Surname
        {
            get => _adaptee["surname"];
            set => _adaptee["surname"] = value;
        }

        public override int Seniority
        {
            get => int.Parse(_adaptee["seniority"]);
            set => _adaptee["engineClass"] = value.ToString();
        }

        public TupleStackDriverAdapter(TupleStackDriver adaptee)
        {
            this._adaptee = adaptee;

            TupleStackRepresentation.Drivers.Add(adaptee.TupleRepr.Item1, this);
        }
    }
}
