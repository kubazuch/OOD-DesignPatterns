using System;
using System.Collections.Generic;

namespace BTM.Builder
{
    public abstract class AbstractBuilder
    {
        protected static readonly Dictionary<string, Func<AbstractBuilder>> Builders = new()
        {
            ["line"] = () => new LineBuilder(),
            ["driver"] = () => new DriverBuilder(),
            ["bytebus"] = () => new BytebusBuilder(),
            ["stop"] = () => new StopBuilder(),
            ["tram"] = () => new TramBuilder()
        };

        protected Dictionary<string, Func<string, AbstractBuilder>> Setters { get; init; }

        public string Name { get; }

        public IEnumerable<string> Fields => Setters.Keys;

        protected AbstractBuilder(string name)
        {
            Name = name;
        }

        public AbstractBuilder Set(string name, string value)
        {
            if (!Setters.TryGetValue(name, out var setter))
                throw new ArgumentException($"Unknown field: {name}. Possible fields: {string.Join(", ", Setters.Keys)}");

            return setter(value);
        }

        public static AbstractBuilder GetByType(string type)
        {
            if (!Builders.TryGetValue(type, out var value))
                throw new ArgumentException($"Unknown entity type: {type}. Possible types: {string.Join(", ", Builders.Keys)}");

            return value();
        }

    }
}
