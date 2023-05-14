using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM.BaseData
{
    public sealed class BaseLine : Line
    {
        public override string NumberHex { get; set; }
        public override int NumberDec { get; set; }
        public override string CommonName { get; set; }
        public override List<Stop> Stops { get; }
        public override List<Vehicle> Vehicles { get; }

        public BaseLine(string numberHex, int numberDec, string commonName)
        {
            NumberHex = numberHex;
            NumberDec = numberDec;
            CommonName = commonName;
            Stops = new List<Stop>();
            Vehicles = new List<Vehicle>();
        }
    }
}