using BTM.Builder;

namespace BTM.BaseData
{
    public class BaseAbstractFactory : AbstractFactory
    {
        public BaseAbstractFactory() : base("base")
        {}

        public override Line CreateLine(LineBuilder builder)
        {
            return new BaseLine(builder.NumberHex!, builder.NumberDec!.Value, builder.CommonName!);
        }

        public override Driver CreateDriver(DriverBuilder builder)
        {
            return new BaseDriver(builder.Name!, builder.Surname!, builder.Seniority!.Value);
        }

        public override Bytebus CreateBytebus(BytebusBuilder builder)
        {
            return new BaseBytebus(builder.Id!.Value, builder.EngineClass!);
        }

        public override Stop CreateStop(StopBuilder builder)
        {
            return new BaseStop(builder.Id!.Value, builder.Name!, builder.Type!);
        }

        public override Tram CreateTram(TramBuilder builder)
        {
            return new BaseTram(builder.Id!.Value, builder.CarsNumber!.Value);
        }
    }
}
