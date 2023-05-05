using BTM.Builder;
using System;

namespace BTM.TupleStackData
{
    public class TupleStackAbstractFactory : AbstractFactory
    {
        public override ILine CreateLine(LineBuilder builder)
        {
            return new TupleStackLineAdapter(new TupleStackLine(builder._numberHex, builder._numberDec, builder._commonName,
                Array.Empty<int>(), Array.Empty<int>()));
        }

        public override IDriver CreateDriver(DriverBuilder builder)
        {
            return new TupleStackDriverAdapter(new TupleStackDriver(builder._name, builder._surname, builder._seniority));
        }

        public override IBytebus CreateBytebus(BytebusBuilder builder)
        {
            return new TupleStackBytebusAdapter(new TupleStackBytebus(builder._id, builder._engineClass));
        }

        public override IStop CreateStop(StopBuilder builder)
        {
            return new TupleStackStopAdapter(new TupleStackStop(builder._id, builder._name, builder._type));
        }

        public override ITram CreateTram(TramBuilder builder)
        {
            return new TupleStackTramAdapter(new TupleStackTram(builder._id, builder._carsNumber, -1));
        }
    }
}
