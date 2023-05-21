using System;
using System.Collections.Generic;

namespace BTM.BaseData
{
    public sealed class BaseLine : Line
    {
        public override string NumberHex { get; set; }

        private int _lineNumber;
        public override int NumberDec
        {
            get => _lineNumber;
            set
            {
                ChangeId(value);
                _lineNumber = value;
            }
        }

        public override string CommonName { get; set; }
        public override List<Stop> Stops { get; }
        public override List<Vehicle> Vehicles { get; }

        public BaseLine(string numberHex, int numberDec, string commonName)
        {
            NumberHex = numberHex;
            NumberDec = numberDec;
            CommonName = commonName;
            Stops = new List<Stop>();
            Vehicles = new List<Vehicle>();
        }

        public override void OnVehicleDeleted(Vehicle vehicle)
        {
            Vehicles.Remove(vehicle);
            vehicle.VehicleDeleted -= OnVehicleDeleted;
        }

        public override void OnStopDeleted(Stop stop)
        {
            Stops.Remove(stop);
            stop.StopDeleted -= OnStopDeleted;
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var vehicle in Vehicles)
                vehicle.VehicleDeleted -= OnVehicleDeleted;

            foreach (var stop in Stops)
                stop.StopDeleted -= OnStopDeleted;

            Stops.Clear();
            Vehicles.Clear();
        }
    }
}