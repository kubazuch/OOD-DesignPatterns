using System.Collections.Generic;

namespace BTM
{
    public interface IBytebus : IVehicle
    {
        public List<ILine> Lines { get; }
        public string EngineClass { get; }
    }
}