using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.Data
{
    public class Bytebus : IBytebus
    {
        public int Id { get; }
        public List<ILine> Lines { get; }
        public string EngineClass { get; }

        public Bytebus(int id, string engineClass, params ILine[] lines)
        {
            Id = id;
            Lines = new List<ILine>();
            EngineClass = engineClass;

            foreach (var line in lines)
            {
                AddLine(line);
            }
        }

        public void AddLine(ILine line)
        {
            Lines.Add(line);
            line.Vehicles.Add(this);
        }

        public object GetValueByName(string name)
        {
            switch (name)
            {
                case "id":
                    return Id;
                case "engineClass":
                    return EngineClass;
                default:
                    throw new ArgumentException($"Unknown field: {name}");
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ByteBus #").Append(Id).Append(", engine class: ").AppendLine(EngineClass);
            builder.Append("\tLines: [").AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("]");
            return builder.ToString();
        }
    }
}