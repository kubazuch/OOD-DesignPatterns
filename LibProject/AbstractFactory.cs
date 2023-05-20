using BTM.Builder;
using System.Collections.Generic;
using BTM.BaseData;
using BTM.TupleStackData;

namespace BTM
{
    public abstract class AbstractFactory
    {
        public string Name { get; }
        public static Dictionary<string, AbstractFactory> Mapping = new ()
        {
            ["base"] = new BaseAbstractFactory(),
            ["secondary"] = new TupleStackAbstractFactory()
        };

        protected AbstractFactory(string name)
        {
            Name = name;
        }

        public abstract Line CreateLine(LineBuilder builder);

        public abstract Driver CreateDriver(DriverBuilder builder);

        public abstract Bytebus CreateBytebus(BytebusBuilder builder);

        public abstract Stop CreateStop(StopBuilder builder);

        public abstract Tram CreateTram(TramBuilder builder);

        public Entity CreateEntity(AbstractBuilder builder) => builder.Build(this);
    }
}
