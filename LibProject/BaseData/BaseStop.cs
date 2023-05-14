using System.Collections.Generic;

namespace BTM.BaseData
{
    public sealed class BaseStop : Stop
    {
        public override int Id { get; set; }
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
        }
    }
}