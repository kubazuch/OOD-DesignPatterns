using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Tram : Vehicle
    {
        public event Action<Tram>? TramDeleted; 

        public new static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int>("id"), new Field<int>("carsNumber")
        }.ToDictionary(f => f.Name, f => f);

        public abstract int CarsNumber { get; set; }
        public abstract Line? Line { get; protected set; }

        protected Tram()
            : base(AvailableFields)
        {
            AssignSettersAndGetters();
        }

        protected void ChangeId(int value)
        {
            if (Vault == null)
                return;

            if (Vault.Trams.ContainsKey(value))
                throw new ArgumentException($"DataVault already contains Tram with id {value}");
            if (Vault.Vehicles.ContainsKey(value))
                throw new ArgumentException($"DataVault already contains Vehicle with id {value}");

            Vault.Trams.Remove(Id);
            Vault.Trams.Add(value, this);

            Vault.Vehicles.Remove(Id);
            Vault.Vehicles.Add(value, this);
        }

        public override void SetVault(DataVault vault) => vault.Register(this);

        public abstract override void OnLineDeleted(Line line);

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var carsNumber = (Field<int>)Fields["carsNumber"];
            carsNumber.SetSetter(value => CarsNumber = value);
            carsNumber.SetGetter(() => CarsNumber);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Tram #").Append(Id).Append(", cars: ").Append(CarsNumber).Append(", line: ").Append(Line?.NumberDec.ToString() ?? "none");
            return builder.ToString();
        }

        public override void Dispose()
        {
            base.Dispose();
            TramDeleted?.Invoke(this);
        }
    }
}