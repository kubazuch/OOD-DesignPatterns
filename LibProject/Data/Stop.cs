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

        public Dictionary<string, Func<object>> Fields { get; }

        public Stop(int id, string name, string type, params ILine[] lines)
        {
            Id = id;
            Lines = new List<ILine>();
            Name = name;
            Type = type;

            Fields = new Dictionary<string, Func<object>>
            {
                ["id"] = () => Id,
                ["name"] = () => Name,
                ["type"] = () => Type
            };

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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Stop #").Append(Id).Append(", \"").Append(Name).Append("\", type: ").AppendLine(Type);
            builder.Append("\tLines: [").AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("]");
            return builder.ToString();
        }
    }
}