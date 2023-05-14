﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM.Builder
{
    public abstract class AbstractBuilder : IRefractive
    {
        protected static readonly Dictionary<string, Func<bool, AbstractBuilder>> Builders = new()
        {
            ["line"] = init => new LineBuilder(init),
            ["driver"] = init => new DriverBuilder(init),
            ["bytebus"] = init => new BytebusBuilder(init),
            ["stop"] = init => new StopBuilder(init),
            ["tram"] = init => new TramBuilder(init)
        };

        public string BuilderName { get; }

        public Dictionary<string, IField> Fields { get; }

        protected AbstractBuilder(string builderName, Dictionary<string, IField> fields)
        {
            BuilderName = builderName;
            Fields = fields.ToDictionary(entry => entry.Key, entry => (IField)entry.Value.Clone());
        }

        public abstract void AssignSettersAndGetters();

        public static AbstractBuilder GetByType(string type, bool init = true)
        {
            if (!Builders.TryGetValue(type, out var value))
                throw new ArgumentException($"Unknown entity type: {type}. Possible types: {string.Join(", ", Builders.Keys)}");

            return value(init);
        }

        public override string ToString() => ToString(false);

        public string ToString(bool color)
        {
            var sb = new StringBuilder();
            foreach (var field in Fields)
            {
                if(field.Value.Value == null) continue;
                sb.Append(field.Key).Append(color ? "=§e\"" : "=\"").Append(field.Value.Value).AppendLine(color ? "\"§r" : "\"");
            }
            return sb.ToString().TrimEnd();
        }

    }
}
