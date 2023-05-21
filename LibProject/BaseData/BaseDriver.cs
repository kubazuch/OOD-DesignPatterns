using System;
using System.Collections.Generic;

namespace BTM.BaseData
{
    public sealed class BaseDriver : Driver
    {
        private int _id;

        public override int Id
        {
            get => _id;
            set
            {
                ChangeId(value);
                _id = value;
            }
        }

        public override List<Vehicle> Vehicles { get; }
        public override string Name { get; set; }
        public override string Surname { get; set; }
        public override int Seniority { get; set; }

        public BaseDriver(int id, string name, string surname, int seniority, params Vehicle[] vehicles)
        {
            Id = id;
            Vehicles = new List<Vehicle>(vehicles);
            Name = name;
            Surname = surname;
            Seniority = seniority;

            foreach (var vehicle in vehicles)
                vehicle.VehicleDeleted += OnVehicleDeleted;
        }

        public override void OnVehicleDeleted(Vehicle vehicle)
        {
            Vehicles.Remove(vehicle);
            vehicle.VehicleDeleted -= OnVehicleDeleted;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            foreach (var vehicle in Vehicles)
                vehicle.VehicleDeleted -= OnVehicleDeleted;

            Vehicles.Clear();
        }
    }
}
