using System;
using System.Collections.Generic;

namespace BTM.BaseData
{
    public sealed class BaseStop : Stop
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
        public override string Name { get; set; }
        public override string Type { get; set; }

        public BaseStop(int id, string name, string type, params Line[] lines)
        {
            Id = id;
            Lines = new List<Line>();
            Name = name;
            Type = type;
            
            foreach (var line in lines)
            {
                AddLine(line);
            }
        }

        public void AddLine(Line line)
        {
            Lines.Add(line);
            line.Stops.Add(this);

            StopDeleted += line.OnStopDeleted;
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

            foreach (var line in Lines)
                line.LineDeleted -= OnLineDeleted;

            Lines.Clear();
        }
    }
}