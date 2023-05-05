namespace BTM.Builder
{
    public class TramBuilder : AbstractBuilder
    {
        internal int _id;
        internal int _carsNumber;

        public TramBuilder() { }

        public TramBuilder SetId(int id)
        {
            this._id = id;
            return this;
        }

        public TramBuilder SetCarsNumber(int number)
        {
            this._carsNumber = number;
            return this;
        }
    }
}
