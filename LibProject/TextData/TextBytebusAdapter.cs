using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public class TextBytebusAdapter : TextVehicleAdapter, IBytebus
    {
        private static Regex BYTEBUS = new Regex(@"#(?<id>\d+)\^(?<engineclass>.+)\*(?:(?<lineid>\d+),?)+", RegexOptions.Compiled);
        
        private TextBytebus adaptee;

        public override int Id => int.Parse(BYTEBUS.Match(adaptee.TextRepr).Groups["id"].Value);
        public List<ILine> Lines => BYTEBUS.Match(adaptee.TextRepr).Groups["lineid"]
            .Captures.Select(id => TextRepresentation.LINES[int.Parse((string) id.Value)]).ToList();
        public string EngineClass => BYTEBUS.Match(adaptee.TextRepr).Groups["engineclass"].Value;

        public TextBytebusAdapter(TextBytebus adaptee)
        {
            this.adaptee = adaptee;

            TextRepresentation.BYTEBUSES.Add(Id, this);
            TextRepresentation.VEHICLES.Add(Id, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Id).Append(", ");
            builder.Append('[').AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("], ");
            builder.Append(EngineClass);
            return builder.ToString();
        }
    }
}