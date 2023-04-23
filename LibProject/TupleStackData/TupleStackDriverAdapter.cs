using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.TupleStackData
{
    public class TupleStackDriverAdapter : IDriver
    {
        private TupleStackDriver adaptee;

        public List<IVehicle> Vehicles
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("vehicles"));
                int cnt = int.Parse(fromStack[i + 1]);
                return fromStack.GetRange(i + 2, cnt).Select(id => TupleStackRepresentation.VEHICLES[int.Parse(id)]).ToList();
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

        public string Surname
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("surname"));

                return fromStack[i + 2];
            }
        }

        public int Seniority
        {
            get
            {
                List<string> fromStack = adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("seniority"));

                return int.Parse(fromStack[i + 2]);
            }
        }

        public TupleStackDriverAdapter(TupleStackDriver adaptee)
        {
            this.adaptee = adaptee;

            TupleStackRepresentation.DRIVERS.Add(adaptee.TupleRepr.Item1, this);
        }

        public object GetValueByName(string name)
        {
            switch (name)
            {
                case "name":
                    return Name;
                case "surname":
                    return Surname;
                case "seniority":
                    return Seniority;
                default:
                    throw new ArgumentException($"Unknown field: {name}");
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name).Append(' ').Append(Surname).Append(", ").Append(Seniority).AppendLine(" years of seniority");
            builder.Append("\tVehicles: [").AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }
    }
}
