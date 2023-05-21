using System;

namespace BTM.BaseData
{
    public sealed class BaseTram : Tram
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

        public override int CarsNumber { get; set; }

        public override Line? Line { get; protected set; }

        public BaseTram(int id, int carsNumber, Line? line = null)
        {
            Id = id;
            CarsNumber = carsNumber;
            Line = line;

            if (line == null) return;
            line.Vehicles.Add(this);
            VehicleDeleted += line.OnVehicleDeleted;
            line.LineDeleted += OnLineDeleted;
        }

        public override void OnLineDeleted(Line line)
        {
            Line = null;
            line.LineDeleted -= OnLineDeleted;
        }

        public override void Dispose()
        {
            base.Dispose();
            if(Line == null) return;

            Line.LineDeleted -= OnLineDeleted;
            Line = null;
        }
    }
}