using BTM.Builder;
using System;
using System.Collections.Generic;

namespace BTM
{
    public abstract class AbstractFactory
    {
        private readonly Dictionary<string, Func<AbstractBuilder, Entity>> _creators;

        protected AbstractFactory()
        {
            _creators = new Dictionary<string, Func<AbstractBuilder, Entity>>
            {
                ["line"] = builder => CreateLine((LineBuilder)builder),
                ["driver"] = builder => CreateDriver((DriverBuilder)builder),
                ["bytebus"] = builder => CreateBytebus((BytebusBuilder)builder),
                ["stop"] = builder => CreateStop((StopBuilder)builder),
                ["tram"] = builder => CreateTram((TramBuilder)builder),
            };
        }

        public abstract Line CreateLine(LineBuilder builder);

        public abstract Driver CreateDriver(DriverBuilder builder);

        public abstract Bytebus CreateBytebus(BytebusBuilder builder);

        public abstract Stop CreateStop(StopBuilder builder);

        public abstract Tram CreateTram(TramBuilder builder);

        public Entity CreateEntity(AbstractBuilder builder)
        {
            if (!_creators.TryGetValue(builder.BuilderName, out var value))
                throw new ArgumentException($"Unknown entity type: {builder.BuilderName}. Possible types: {string.Join(", ", _creators.Keys)}");

            return value(builder);
        }
    }
}
