using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM
{
    public interface ILine
    {
        public string NumberHex { get; }
        public int NumberDec { get; }
        public string CommonName { get; }
        public List<IStop> Stops { get; }
        public List<IVehicle> Vehicles { get; }
    }
}
