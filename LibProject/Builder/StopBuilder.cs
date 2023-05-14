using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM.Builder
{
    public class StopBuilder : AbstractBuilder
    {
        public static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int?>("id"), new Field<string?>("name"), new Field<string?>("type")
        }.ToDictionary(f => f.Name, f => f);

        internal int? Id;
        internal string? Name;
        internal string? Type;

        public StopBuilder(bool init = true) : base("stop", AvailableFields)
        {
            AssignSettersAndGetters();

            if (!init) return;

            Id = 0;
            Name = "City Centre";
            Type = "bus";
        }

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int?>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var name = (Field<string?>)Fields["name"];
            name.SetSetter(value => Name = value);
            name.SetGetter(() => Name);

            var type = (Field<string?>)Fields["type"];
            type.SetSetter(value => Type = value);
            type.SetGetter(() => Type);
        }

        public StopBuilder SetId(int id)
        {
            this.Id = id;
            return this;
        }

        public StopBuilder SetName(string name)
        {
            this.Name = name;
            return this;
        }

        public StopBuilder SetType(string type)
        {
            this.Type = type;
            return this;
        }
    }
}
