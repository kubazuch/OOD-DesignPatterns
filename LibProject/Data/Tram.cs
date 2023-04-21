using System.Text;

namespace BTM.Data
{
    public class Tram : ITram
    {
        private readonly ILine _line;

        public int Id { get; }

        public int CarsNumber { get; }

        public ILine Line
        {
            get => _line;
            private init
            {
                _line = value;
                value.Vehicles.Add(this);
            }
        }

        public Tram(int id, int carsNumber, ILine line)
        {
            Id = id;
            CarsNumber = carsNumber;
            Line = line;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Id).Append(", ");
            builder.Append(CarsNumber).Append(", ");
            builder.Append(Line.NumberDec);
            return builder.ToString();
        }
    }
}