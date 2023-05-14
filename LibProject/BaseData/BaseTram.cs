namespace BTM.BaseData
{
    public sealed class BaseTram : Tram
    {
        public override int Id { get; set; }
        public override int CarsNumber { get; set; }

        public override Line? Line { get; }

        public BaseTram(int id, int carsNumber, Line? line = null)
        {
            Id = id;
            CarsNumber = carsNumber;
            Line = line;
            line?.Vehicles.Add(this);
        }
    }
}