using BTM.Builder;

namespace BTM.Data
{
    public class BaseFactory : IFactory
    {
        public ILine CreateLine(LineBuilder builder)
        {
            return new Line(builder._numberHex, builder._numberDec, builder._commonName);
        }

        public IDriver CreateDriver(DriverBuilder builder)
        {
            return new Driver(builder._name, builder._surname, builder._seniority);
        }

        public IBytebus CreateBytebus(BytebusBuilder builder)
        {
            return new Bytebus(builder._id, builder._engineClass);
        }

        public IStop CreateStop(StopBuilder builder)
        {
            return new Stop(builder._id, builder._name, builder._type);
        }

        public ITram CreateTram(TramBuilder builder)
        {
            return new Tram(builder._id, builder._carsNumber, null);
        }
    }
}
