using System;
using System.Collections.Generic;

namespace BTM.BaseData
{
    public sealed class BaseBytebus : Bytebus
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

        public override List<Line> Lines { get; }
        public override string EngineClass { get; set; }

        public BaseBytebus(int id, string engineClass, params Line[] lines)
        {
            Id = id;
            Lines = new List<Line>();
            EngineClass = engineClass;

            foreach (var line in lines)
            {
                AddLine(line);
            }
        }

        public void AddLine(Line line)
        {
            Lines.Add(line);
            line.Vehicles.Add(this);

            VehicleDeleted += line.OnVehicleDeleted;
            line.LineDeleted += OnLineDeleted;
        }

        public override void OnLineDeleted(Line line)
        {
            Lines.Remove(line);
            line.LineDeleted -= OnLineDeleted;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            foreach(var line in Lines)
                line.LineDeleted -= OnLineDeleted;

            Lines.Clear();
        }
    }
}