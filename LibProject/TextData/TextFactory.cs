using BTM.Builder;
using System;

namespace BTM.TextData
{
    public class TextAbstractFactory : AbstractFactory
    {
        public override ILine CreateLine(LineBuilder builder)
        {
            return new TextLineAdapter(new TextLine(builder._numberHex, builder._numberDec, builder._commonName,
                Array.Empty<int>(), Array.Empty<int>()));
        }

        public override IDriver CreateDriver(DriverBuilder builder)
        {
            return new TextDriverAdapter(new TextDriver(builder._name, builder._surname, builder._seniority));
        }

        public override IBytebus CreateBytebus(BytebusBuilder builder)
        {
            return new TextBytebusAdapter(new TextBytebus(builder._id, builder._engineClass));
        }

        public override IStop CreateStop(StopBuilder builder)
        {
            return new TextStopAdapter(new TextStop(builder._id, builder._name, builder._type));
        }

        public override ITram CreateTram(TramBuilder builder)
        {
            return new TextTramAdapter(new TextTram(builder._id, builder._carsNumber, -1));
        }
    }
}
