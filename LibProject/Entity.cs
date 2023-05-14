using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM
{
    public abstract class Entity : IRefractive
    {
        public Dictionary<string, IField> Fields { get; }

        protected Entity(Dictionary<string, IField> fields)
        {
            Fields = fields.ToDictionary(entry => entry.Key, entry => (IField)entry.Value.Clone());
        }
        public abstract void AssignSettersAndGetters();

        public static readonly Dictionary<string, Dictionary<string, IField>> AvailableFields = new()
        {
            ["line"]    = Line.AvailableFields,
            ["stop"]    = Stop.AvailableFields,
            ["bytebus"] = Bytebus.AvailableFields,
            ["tram"]    = Tram.AvailableFields,
            ["driver"]  = Driver.AvailableFields
        };

        public abstract override string ToString();
    }
}
