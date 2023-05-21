using System;
using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM
{
    public abstract class Entity : IRefractive, IDisposable
    {
        public Dictionary<string, IField> Fields { get; }

        internal DataVault? Vault;

        protected Entity(Dictionary<string, IField> fields)
        {
            Fields = fields.ToDictionary(entry => entry.Key, entry => (IField)entry.Value.Clone());
        }

        public abstract void AssignSettersAndGetters();

        public abstract void SetVault(DataVault vault);

        public static readonly Dictionary<string, Dictionary<string, IField>> AvailableFields = new()
        {
            ["line"]    = Line.AvailableFields,
            ["stop"]    = Stop.AvailableFields,
            ["bytebus"] = Bytebus.AvailableFields,
            ["tram"]    = Tram.AvailableFields,
            ["driver"]  = Driver.AvailableFields
        };

        public abstract override string ToString();

        public abstract void Dispose();
    }
}
