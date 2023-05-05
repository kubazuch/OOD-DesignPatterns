using BTM.Builder;

namespace BTM
{
    public interface IFactory
    {
        public ILine CreateLine(LineBuilder builder);

        public IDriver CreateDriver(DriverBuilder builder);

        public IBytebus CreateBytebus(BytebusBuilder builder);

        public IStop CreateStop(StopBuilder builder);

        public ITram CreateTram(TramBuilder builder);
    }
}
