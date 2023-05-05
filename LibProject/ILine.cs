using System.Collections.Generic;

namespace BTM
{
    public interface ILine : IEntity
    {
        public string NumberHex { get; }
        public int NumberDec { get; }
        public string CommonName { get; }
        public List<IStop> Stops { get; }
        public List<IVehicle> Vehicles { get; }
    }
}
