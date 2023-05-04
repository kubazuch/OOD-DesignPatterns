using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM
{
    public interface IEntity
    {
        public Dictionary<string, Func<object>> Fields { get; }

        public object GetValueByName(string name)
        {
            if(!Fields.ContainsKey(name))
                throw new ArgumentException($"Unknown field: {name}. Possible types: {string.Join(", ", Fields.Keys)}");

            return Fields[name]();
        }
    }
}
