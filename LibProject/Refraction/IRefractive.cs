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
                throw new ArgumentException($"Unknown field: `§l{name}§l`. Possible names: {string.Join(", ", Fields.Keys)}");

            return Fields[name].Value;
        }

        void SetValueByName(string name, object? value)
        {
            if (!Fields.ContainsKey(name))
                throw new ArgumentException($"Unknown field: `§l{name}§l`. Possible names: {string.Join(", ", Fields.Keys)}");
            try
            {
                Fields[name].Value = value;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Unable to assign value `§l{value}§l` to field `§l{name}§l`", ex);
            }
        }

        public object? this[string name]
        {
            set => SetValueByName(name, value);
            get => GetValueByName(name);
        }
    }
}
