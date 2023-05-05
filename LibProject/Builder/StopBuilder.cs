using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.Builder
{
    public class StopBuilder
    {
        internal int _id;
        internal string _name;
        internal string _type;

        public StopBuilder() {}

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
