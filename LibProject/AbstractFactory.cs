using System;
using System.Collections.Generic;
using BTM.Builder;

namespace BTM
{
    public abstract class AbstractFactory
    {
        private readonly Dictionary<string, Func<AbstractBuilder, IEntity>> _creators;

        protected AbstractFactory()
        {
            _creators = new Dictionary<string, Func<AbstractBuilder, IEntity>>
            {
                ["line"] = builder => CreateLine((LineBuilder) builder),
                ["driver"] = builder => CreateDriver((DriverBuilder)builder),
                ["bytebus"] = builder => CreateBytebus((BytebusBuilder)builder),
                ["stop"] = builder => CreateStop((StopBuilder)builder),
                ["tram"] = builder => CreateTram((TramBuilder)builder),
            };
        }

        public abstract ILine CreateLine(LineBuilder builder);

        public abstract IDriver CreateDriver(DriverBuilder builder);

        public abstract IBytebus CreateBytebus(BytebusBuilder builder);

        public abstract IStop CreateStop(StopBuilder builder);

        public abstract ITram CreateTram(TramBuilder builder);

        public IEntity CreateEntity(string name, AbstractBuilder builder)
        {
            if (!_creators.TryGetValue(name, out var value))
                throw new ArgumentException($"Unknown entity type: {name}. Possible types: {string.Join(", ", _creators.Keys)}");

            return value(builder);
        }
    }
}
