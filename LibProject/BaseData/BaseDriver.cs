using System.Collections.Generic;

namespace BTM.BaseData
{
    public sealed class BaseDriver : Driver
    {
        public override List<Vehicle> Vehicles { get; }
        public override string Name { get; set; }
        public override string Surname { get; set; }
        public override int Seniority { get; set; }

        public BaseDriver(string name, string surname, int seniority, params Vehicle[] vehicles)
        {
            Vehicles = new List<Vehicle>(vehicles);
            Name = name;
            Surname = surname;
            Seniority = seniority;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            Vehicles.Add(vehicle);
        }
    }
}
