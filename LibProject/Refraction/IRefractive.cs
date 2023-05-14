using System;
using System.Collections.Generic;

namespace BTM.Refraction
{
    public interface IRefractive
    {
        Dictionary<string, IField> Fields { get; }

        void AssignSettersAndGetters();

        object? GetValueByName(string name)
        {
            if (!Fields.ContainsKey(name))
                throw new ArgumentException($"Unknown field: {name}. Possible names: {string.Join(", ", Fields.Keys)}");

            return Fields[name].Value;
        }

        void SetValueByName(string name, object? value)
        {
            if (!Fields.ContainsKey(name))
                throw new ArgumentException($"Unknown field: {name}. Possible names: {string.Join(", ", Fields.Keys)}");

            Fields[name].Value = value;
        }
    }
}
