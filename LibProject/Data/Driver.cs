using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.Data
{
    //--------------------------------------------------//

    public class Driver : IDriver
    {
        public List<IVehicle> Vehicles { get; }
        public string Name { get; }
        public string Surname { get; }
        public int Seniority { get; }

        public Driver(string name, string surname, int seniority, params IVehicle[] vehicles)
        {
            Vehicles = new List<IVehicle>(vehicles);
            Name = name;
            Surname = surname;
            Seniority = seniority;
        }

        public void AddVehicle(IVehicle vehicle)
        {
            Vehicles.Add(vehicle);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name).Append(' ').Append(Surname).Append(", ");
            builder.Append(Seniority).Append(", ");
            builder.Append('[').AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }
    }
}
