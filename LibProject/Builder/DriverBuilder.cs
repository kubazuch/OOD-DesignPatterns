using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM.Builder
{
    public class DriverBuilder : AbstractBuilder
    {
        public static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int?>("id"), new Field<string?>("name"), new Field<string?>("surname"), new Field<int?>("seniority")
        }.ToDictionary(f => f.Name, f => f);

        internal int? Id;
        internal string? Name;
        internal string? Surname;
        internal int? Seniority;

        public DriverBuilder(bool init = true) : base("driver", AvailableFields)
        {
            AssignSettersAndGetters();

            if (!init) return;

            Id = 0;
            Name = "John";
            Surname = "Smith";
            Seniority = 30;
        }
        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int?>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var name = (Field<string?>)Fields["name"];
            name.SetSetter(value => Name = value);
            name.SetGetter(() => Name);

            var surname = (Field<string?>)Fields["surname"];
            surname.SetSetter(value => Surname = value);
            surname.SetGetter(() => Surname);

            var seniority = (Field<int?>)Fields["seniority"];
            seniority.SetSetter(value => Seniority = value);
            seniority.SetGetter(() => Seniority);
        }

        public DriverBuilder SetId(int id)
        {
            this.Id = id;
            return this;
        }

        public DriverBuilder SetName(string name)
        {
            this.Name = name;
            return this;
        }

        public DriverBuilder SetSurname(string surname)
        {
            this.Surname = surname;
            return this;
        }

        public DriverBuilder SetSeniority(int seniority)
        {
            this.Seniority = seniority;
            return this;
        }

        public override Entity Build(AbstractFactory abstractFactory) => abstractFactory.CreateDriver(this);
    }
}
