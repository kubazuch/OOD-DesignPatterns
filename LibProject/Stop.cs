using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Stop : Entity
    {
        public event Action<Stop>? StopDeleted; 

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

        protected void ChangeId(int value)
        {
            if (Vault == null)
                return;

            if (Vault.Stops.ContainsKey(value))
                throw new ArgumentException($"DataVault already contains Stop with id {value}");

            Vault.Stops.Remove(Id);
            Vault.Stops.Add(value, this);
        }

        public override void SetVault(DataVault vault) => vault.Register(this);

        public abstract void OnLineDeleted(Line line);

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

        public override void Dispose() => StopDeleted?.Invoke(this);
    }
}