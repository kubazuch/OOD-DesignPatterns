using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public class TextLineAdapter : ILine
    {
        private static Regex LINE = new Regex(@"(?<numerHex>[0-9A-Z]+)\((?<numerDec>[0-9]+)\)`(?<commonName>.+)`@(?:(?<stopid>\d+),?)+!(?:(?<vehicleid>\d+),?)+", RegexOptions.Compiled);

        private TextLine adaptee;

        public string NumberHex => LINE.Match(adaptee.TextRepr).Groups["numerHex"].Value;

        public int NumberDec => int.Parse(LINE.Match(adaptee.TextRepr).Groups["numerDec"].Value);
        public string CommonName => LINE.Match(adaptee.TextRepr).Groups["commonName"].Value;
        public List<IStop> Stops => LINE.Match(adaptee.TextRepr).Groups["stopid"]
            .Captures.Select(id => TextRepresentation.STOPS[int.Parse(id.Value)]).ToList();

        public List<IVehicle> Vehicles => LINE.Match(adaptee.TextRepr).Groups["vehicleid"]
            .Captures.Select(id => TextRepresentation.VEHICLES[int.Parse(id.Value)]).ToList();

        public TextLineAdapter(TextLine adaptee)
        {
            this.adaptee = adaptee;

            Fields = new Dictionary<string, Func<object>>
            {
                ["numberHex"] = () => NumberHex,
                ["numberDec"] = () => NumberDec,
                ["commonName"] = () => CommonName
            };

            TextRepresentation.LINES.Add(NumberDec, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Line 0x").Append(NumberHex).Append(" (").Append(NumberDec).Append("), \"").Append(CommonName).AppendLine("\"");
            builder.Append("\tStops: [").AppendJoin(", ", Stops.Select(x => x.Id)).AppendLine("]");
            builder.Append("\tVehicles: [").AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }

        public Dictionary<string, Func<object>> Fields { get; }
    }
}