using System.Collections.Generic;

namespace BTM.BaseData
{
    public sealed class BaseBytebus : Bytebus
    {
        public override int Id { get; set; }
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
        }
    }
}