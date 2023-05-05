using BTM.Builder;
using BTM.TextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.TupleStackData
{
    public class TupleStackFactory : IFactory
    {
        public ILine CreateLine(LineBuilder builder)
        {
            return new TupleStackLineAdapter(new TupleStackLine(builder._numberHex, builder._numberDec, builder._commonName,
                Array.Empty<int>(), Array.Empty<int>()));
        }

        public IDriver CreateDriver(DriverBuilder builder)
        {
            return new TupleStackDriverAdapter(new TupleStackDriver(builder._name, builder._surname, builder._seniority));
        }

        public IBytebus CreateBytebus(BytebusBuilder builder)
        {
            return new TupleStackBytebusAdapter(new TupleStackBytebus(builder._id, builder._engineClass));
        }

        public IStop CreateStop(StopBuilder builder)
        {
            return new TupleStackStopAdapter(new TupleStackStop(builder._id, builder._name, builder._type));
        }

        public ITram CreateTram(TramBuilder builder)
        {
            return new TupleStackTramAdapter(new TupleStackTram(builder._id, builder._carsNumber, -1));
        }
    }
}
