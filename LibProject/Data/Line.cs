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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(NumberHex).Append(", ");
            builder.Append(NumberDec).Append(", ");
            builder.Append(CommonName).Append(", ");
            builder.Append('[').AppendJoin(", ", Stops.Select(x => x.Id)).Append(']').Append(", ");
            builder.Append('[').AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }
    }
}