using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTM.Data
{
    public class Line : ILine
    {
        public string NumberHex { get; }
        public int NumberDec { get; }
        public string CommonName { get; }
        public List<IStop> Stops { get; }
        public List<IVehicle> Vehicles { get; }

        public Line(string numberHex, int numberDec, string commonName)
        {
            NumberHex = numberHex;
            NumberDec = numberDec;
            CommonName = commonName;
            Stops = new List<IStop>();
            Vehicles = new List<IVehicle>();
        }

        public object GetValueByName(string name)
        {
            switch (name)
            {
                case "numberHex":
                    return NumberHex;
                case "numberDec":
                    return NumberDec;
                case "commonName":
                    return CommonName;
                default:
                    throw new ArgumentException($"Unknown field: {name}");
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Line 0x").Append(NumberHex).Append(" (").Append(NumberDec).Append("), \"").Append(CommonName).AppendLine("\"");
            builder.Append("\tStops: [").AppendJoin(", ", Stops.Select(x => x.Id)).AppendLine("]");
            builder.Append("\tVehicles: [").AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }
    }
}