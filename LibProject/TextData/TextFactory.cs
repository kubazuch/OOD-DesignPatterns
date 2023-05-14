using BTM.Builder;
using System;

namespace BTM.TextData
{
    public class TextAbstractFactory : AbstractFactory
    {
        public override Line CreateLine(LineBuilder builder)
        {
            return new TextLineAdapter(new TextLine(builder.NumberHex!, builder.NumberDec!.Value, builder.CommonName!,
                Array.Empty<int>(), Array.Empty<int>()));
        }

        public override Driver CreateDriver(DriverBuilder builder)
        {
            return new TextDriverAdapter(new TextDriver(builder.Name!, builder.Surname!, builder.Seniority!.Value));
        }

        public override Bytebus CreateBytebus(BytebusBuilder builder)
        {
            return new TextBytebusAdapter(new TextBytebus(builder.Id!.Value, builder.EngineClass!));
        }

        public override Stop CreateStop(StopBuilder builder)
        {
            return new TextStopAdapter(new TextStop(builder.Id!.Value, builder.Name!, builder.Type!));
        }

        public override Tram CreateTram(TramBuilder builder)
        {
            return new TextTramAdapter(new TextTram(builder.Id!.Value, builder.CarsNumber!.Value, -1));
        }
    }
}
