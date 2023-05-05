namespace BTM.Builder
{
    public class BytebusBuilder
    {
        internal int _id;
        internal string _engineClass;

        public BytebusBuilder() { }

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
