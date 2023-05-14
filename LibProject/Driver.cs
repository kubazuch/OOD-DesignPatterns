using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Driver : Entity
    {
        public new static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<string>("name"), new Field<string>("surname"), new Field<int>("seniority")
        }.ToDictionary(f => f.Name, f => f);

        public abstract List<Vehicle> Vehicles { get; }
        public abstract string Name { get; set; }
        public abstract string Surname { get; set; }
        public abstract int Seniority { get; set; }

        protected Driver()
            : base(AvailableFields)
        {
            AssignSettersAndGetters();
        }

        public sealed override void AssignSettersAndGetters()
        {
            var name = (Field<string>)Fields["name"];
            name.SetSetter(value => Name = value);
            name.SetGetter(() => Name);

            var surname = (Field<string>)Fields["surname"];
            surname.SetSetter(value => Surname = value);
            surname.SetGetter(() => Surname);

            var seniority = (Field<int>)Fields["seniority"];
            seniority.SetSetter(value => Seniority = value);
            seniority.SetGetter(() => Seniority);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Name).Append(' ').Append(Surname).Append(", ").Append(Seniority).AppendLine(" years of seniority");
            builder.Append("\tVehicles: [").AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }
    }
}