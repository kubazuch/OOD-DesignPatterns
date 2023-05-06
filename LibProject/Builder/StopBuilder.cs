using System;
using System.Collections.Generic;

namespace BTM.Builder
{
    public class StopBuilder : AbstractBuilder
    {
        internal int _id = 0;
        internal string _name = "City Centre";
        internal string _type = "bus";

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
