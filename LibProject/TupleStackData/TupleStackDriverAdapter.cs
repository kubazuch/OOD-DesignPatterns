using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("name"));

                return fromStack[i + 2];
            }
            set => throw new NotImplementedException();
        }

        public override string Surname
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("surname"));

                return fromStack[i + 2];
            }
            set => throw new NotImplementedException();
        }

        public override int Seniority
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("seniority"));

                return int.Parse(fromStack[i + 2]);
            }
            set => throw new NotImplementedException();
        }

        public TupleStackDriverAdapter(TupleStackDriver adaptee)
        {
            this._adaptee = adaptee;

            TupleStackRepresentation.Drivers.Add(adaptee.TupleRepr.Item1, this);
        }
    }
}
