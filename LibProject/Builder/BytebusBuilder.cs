using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM.Builder
{
    public class BytebusBuilder : AbstractBuilder
    {
        public static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int?>("id"), new Field<string?>("engineClass")
        }.ToDictionary(f => f.Name, f => f);

        internal int? Id;
        internal string? EngineClass;

        public BytebusBuilder(bool init = true) : base("bytebus", AvailableFields)
        {
            AssignSettersAndGetters();

            if (!init) return;

            Id = 0;
            EngineClass = "Byte5";
        }

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int?>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var engineClass = (Field<string?>)Fields["engineClass"];
            engineClass.SetSetter(value => EngineClass = value);
            engineClass.SetGetter(() => EngineClass);
        }

        public BytebusBuilder SetId(int id)
        {
            this.Id = id;
            return this;
        }

        public BytebusBuilder SetEngineClass(string engineClass)
        {
            this.EngineClass = engineClass;
            return this;
        }

        public override Entity Build(AbstractFactory abstractFactory) => abstractFactory.CreateBytebus(this);
    }
}
