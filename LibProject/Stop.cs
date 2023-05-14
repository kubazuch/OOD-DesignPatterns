using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Stop : Entity
    {
        public new static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int>("id"), new Field<string>("name"), new Field<string>("type")
        }.ToDictionary(f => f.Name, f => f);

        public abstract int Id { get; set; }
        public abstract List<Line> Lines { get; }
        public abstract string Name { get; set; }
        public abstract string Type { get; set; }

        public Stop()
            : base(AvailableFields)
        {
            AssignSettersAndGetters();
        }

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var name = (Field<string>)Fields["name"];
            name.SetSetter(value => Name = value);
            name.SetGetter(() => Name);

            var type = (Field<string>)Fields["type"];
            type.SetSetter(value => Type = value);
            type.SetGetter(() => Type);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Stop #").Append(Id).Append(", \"").Append(Name).Append("\", type: ").AppendLine(Type);
            builder.Append("\tLines: [").AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("]");
            return builder.ToString();
        }
    }
}