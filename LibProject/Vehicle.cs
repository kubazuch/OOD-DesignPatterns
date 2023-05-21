using System;
using System.Collections.Generic;
using BTM.Refraction;

namespace BTM
{
    public abstract class Vehicle : Entity
    {
        public event Action<Vehicle>? VehicleDeleted;
        
        protected Vehicle(Dictionary<string, IField> fields) : base(fields) {}

        public abstract int Id { get; set; }
        
        public abstract void OnLineDeleted(Line line);

        public override void Dispose() => VehicleDeleted?.Invoke(this);
    }
}