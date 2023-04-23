using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public class TextStopAdapter : IStop
    {
        private static Regex STOP = new Regex(@"#(?<id>\d+)\((?:(?<lineid>\d+),?)+\)(?<name>.+)/(?<type>\w+)", RegexOptions.Compiled);
        
        private TextStop adaptee;

        public int Id => int.Parse(STOP.Match(adaptee.TextRepr).Groups["id"].Value);
        public List<ILine> Lines => STOP.Match(adaptee.TextRepr).Groups["lineid"]
            .Captures.Select(id => TextRepresentation.LINES[int.Parse((string) id.Value)]).ToList();

        public string Name => STOP.Match(adaptee.TextRepr).Groups["name"].Value;
        public string Type => STOP.Match(adaptee.TextRepr).Groups["type"].Value;

        public TextStopAdapter(TextStop adaptee)
        {
            this.adaptee = adaptee;

            TextRepresentation.STOPS.Add(Id, this);
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