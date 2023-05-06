using System;
using System.Collections.Generic;

namespace BTM.Builder
{
    public class LineBuilder : AbstractBuilder
    {
        internal string _numberHex = "0";
        internal int _numberDec = 0;
        internal string _commonName = "Bus line";

        public LineBuilder() : base("line")
        {
            Setters = new Dictionary<string, Func<string, AbstractBuilder>>
            {
                ["numberHex"] = SetNumberHex,
                ["numberDec"] = v => SetNumberDec(int.Parse(v)),
                ["commonName"] = SetCommonName
            };
        }

        public LineBuilder SetNumberHex(string numberHex)
        {
            this._numberHex = numberHex;
            return this;
        }

        public LineBuilder SetNumberDec(int numberDec)
        {
            this._numberDec = numberDec;
            return this;
        }

        public LineBuilder SetCommonName(string commonName)
        {
            this._commonName = commonName;
            return this;
        }
    }
}
