using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM.Builder
{
    public class DriverBuilder : AbstractBuilder
    {
        public static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<string?>("name"), new Field<string?>("surname"), new Field<int?>("seniority")
        }.ToDictionary(f => f.Name, f => f);

        internal string? Name;
        internal string? Surname;
        internal int? Seniority;

        public DriverBuilder(bool init = true) : base("driver", AvailableFields)
        {
            AssignSettersAndGetters();

            if (!init) return;

            Name = "John";
            Surname = "Smith";
            Seniority = 30;
        }
        public sealed override void AssignSettersAndGetters()
        {
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
    }
}
