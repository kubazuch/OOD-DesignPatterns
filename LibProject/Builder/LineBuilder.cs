namespace BTM.Builder
{
    public class LineBuilder : AbstractBuilder
    {
        internal string _numberHex;
        internal int _numberDec;
        internal string _commonName;

        public LineBuilder() { }

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
