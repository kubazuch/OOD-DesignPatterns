using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.Data
{
    public class Stop : IStop
    {
        public int Id { get; }
        public List<ILine> Lines { get; }
        public string Name { get; }
        public string Type { get; }

        public Stop(int id, string name, string type, params ILine[] lines)
        {
            Id = id;
            Lines = new List<ILine>();
            Name = name;
            Type = type;

            foreach (var line in lines)
            {
                AddLine(line);
            }
        }

        public void AddLine(ILine line)
        {
            Lines.Add(line);
            line.Stops.Add(this);
        }

        public object GetValueByName(string name)
        {
            switch (name)
            {
                case "id":
                    return Id;
                case "name":
                    return Name;
                case "type":
                    return Type;
                default:
                    throw new ArgumentException($"Unknown field: {name}");
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Stop #").Append(Id).Append(", \"").Append(Name).Append("\", type: ").AppendLine(Type);
            builder.Append("\tLines: [").AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("]");
            return builder.ToString();
        }
    }
}