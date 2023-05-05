using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.Builder
{
    public class DriverBuilder
    {
        internal string _name;
        internal string _surname;
        internal int _seniority;

        internal DriverBuilder()
        {
        }

        public DriverBuilder SetName(string name)
        {
            this._name = name;
            return this;
        }

        public DriverBuilder SetSurname(string surname)
        {
            this._surname = surname;
            return this;
        }

        public DriverBuilder SetSeniority(int seniority)
        {
            this._seniority = seniority;
            return this;
        }
    }
}
