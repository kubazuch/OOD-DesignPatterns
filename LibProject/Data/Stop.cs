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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Id).Append(", ");
            builder.Append(Name).Append(", ");
            builder.Append(Type).Append(", ");
            builder.Append('[').AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append(']');
            return builder.ToString();
        }
    }
}