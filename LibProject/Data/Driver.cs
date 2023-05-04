using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.Data
{
    public class Driver : IDriver
    {
        public List<IVehicle> Vehicles { get; }
        public string Name { get; }
        public string Surname { get; }
        public int Seniority { get; }

        public Dictionary<string, Func<object>> Fields { get; }

        public Driver(string name, string surname, int seniority, params IVehicle[] vehicles)
        {
            Vehicles = new List<IVehicle>(vehicles);
            Name = name;
            Surname = surname;
            Seniority = seniority;

            Fields = new Dictionary<string, Func<object>>()
            {
                ["name"] = () => Name,
                ["surname"] = () => Surname,
                ["seniority"] = () => Seniority
            };
        }

        public void AddVehicle(IVehicle vehicle)
        {
            Vehicles.Add(vehicle);
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
