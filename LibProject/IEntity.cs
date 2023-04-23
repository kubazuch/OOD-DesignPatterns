using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM
{
    public interface IEntity
    {
        public object GetValueByName(string name);
    }
}
