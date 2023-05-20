using BTM.Builder;
using System;

namespace BTM.TupleStackData
{
    public class TupleStackAbstractFactory : AbstractFactory
    {
        public TupleStackAbstractFactory() : base("secondary")
        { }

        public override Line CreateLine(LineBuilder builder)
        {
            return new TupleStackLineAdapter(new TupleStackLine(builder.NumberHex!, builder.NumberDec!.Value, builder.CommonName!,
                Array.Empty<int>(), Array.Empty<int>()));
        }

        public override Driver CreateDriver(DriverBuilder builder)
        {
            return new TupleStackDriverAdapter(new TupleStackDriver(builder.Name!, builder.Surname!, builder.Seniority!.Value));
        }

        public override Bytebus CreateBytebus(BytebusBuilder builder)
        {
            return new TupleStackBytebusAdapter(new TupleStackBytebus(builder.Id!.Value, builder.EngineClass!));
        }

        public override Stop CreateStop(StopBuilder builder)
        {
            return new TupleStackStopAdapter(new TupleStackStop(builder.Id!.Value, builder.Name!, builder.Type!));
        }

        public override Tram CreateTram(TramBuilder builder)
        {
            return new TupleStackTramAdapter(new TupleStackTram(builder.Id!.Value, builder.CarsNumber!.Value, -1));
        }
    }
}
