using System;
using System.Collections.Generic;

namespace BTM.Builder
{
    public class StopBuilder : AbstractBuilder
    {
        internal int _id;
        internal string _name;
        internal string _type;

        public StopBuilder() : base("stop")
        {
            Setters = new Dictionary<string, Func<string, AbstractBuilder>>
            {
                ["id"] = v => SetId(int.Parse(v)),
                ["name"] = SetName,
                ["type"] = SetType
            };
        }

        public StopBuilder SetId(int id)
        {
            this._id = id;
            return this;
        }

        public StopBuilder SetName(string name)
        {
            this._name = name;
            return this;
        }

        public StopBuilder SetType(string type)
        {
            this._type = type;
            return this;
        }
    }
}
