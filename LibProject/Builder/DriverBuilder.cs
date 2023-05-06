using System;
using System.Collections.Generic;

namespace BTM.Builder
{
    public class DriverBuilder : AbstractBuilder
    {
        internal string _name;
        internal string _surname;
        internal int _seniority;

        public DriverBuilder() : base("driver")
        {
            Setters = new Dictionary<string, Func<string, AbstractBuilder>>
            {
                ["name"] = SetName,
                ["surname"] = SetSurname,
                ["seniority"] = v => SetSeniority(int.Parse(v))
            };
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
