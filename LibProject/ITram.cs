namespace BTM
{
    public interface ITram : IVehicle
    {
        public int CarsNumber { get; }
        public ILine Line { get; }
    }
}