using System.Collections.Generic;
using BTM.Refraction;

namespace BTM
{
    public abstract class Vehicle : Entity
    {
        protected Vehicle(Dictionary<string, IField> fields) : base(fields) {}

        public abstract int Id { get; set; }
    }
}