using System;
using System.Collections.Generic;

namespace BTM.Builder
{
    public class BytebusBuilder : AbstractBuilder
    {
        internal int _id = 0;
        internal string _engineClass = "Byte5";

        public BytebusBuilder() : base("bytebus")
        {
            Setters = new Dictionary<string, Func<string, AbstractBuilder>>
            {
                ["id"] = v => SetId(int.Parse(v)),
                ["engineClass"] = SetEngineClass
            };
        }

        public BytebusBuilder SetId(int id)
        {
            this._id = id;
            return this;
        }

        public BytebusBuilder SetEngineClass(string engineClass)
        {
            this._engineClass = engineClass;
            return this;
        }
    }
}
