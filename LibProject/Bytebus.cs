using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Bytebus : Vehicle
    {
        public event Action<Bytebus>? BytebusDeleted; 

        public new static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int>("id"), new Field<string>("engineClass")
        }.ToDictionary(f => f.Name, f => f);

        public abstract List<Line> Lines { get; }

        public abstract string EngineClass { get; set; }

        protected Bytebus()
            : base(AvailableFields)
        {
            AssignSettersAndGetters();
        }

        protected void ChangeId(int value)
        {
            if (Vault == null)
                return;

            if (Vault.Bytebuses.ContainsKey(value))
                throw new ArgumentException($"DataVault already contains Bytebus with id {value}");
            if (Vault.Vehicles.ContainsKey(value))
                throw new ArgumentException($"DataVault already contains Vehicle with id {value}");

            Vault.Bytebuses.Remove(Id);
            Vault.Bytebuses.Add(value, this);

            Vault.Vehicles.Remove(Id);
            Vault.Bytebuses.Add(value, this);
        }

        public override void SetVault(DataVault vault) => vault.Register(this);

        public abstract override void OnLineDeleted(Line line);

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var engineClass = (Field<string>)Fields["engineClass"];
            engineClass.SetSetter(value => EngineClass = value);
            engineClass.SetGetter(() => EngineClass);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ByteBus #").Append(Id).Append(", engine class: ").AppendLine(EngineClass);
            builder.Append("\tLines: [").AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("]");
            return builder.ToString();
        }

        public override void Dispose()
        {
            base.Dispose();
            BytebusDeleted?.Invoke(this);
        }
    }
}