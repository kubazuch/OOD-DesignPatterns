using System;
using System.Collections.Generic;
using System.Text;

namespace BTM.Data
{
    public class Tram : ITram
    {
        private readonly ILine _line;

        public int Id { get; }

        public int CarsNumber { get; }

        public Dictionary<string, Func<object>> Fields { get; }

        public ILine Line
        {
            get => _line;
            private init
            {
                _line = value;
                value?.Vehicles.Add(this);
            }
        }

        public Tram(int id, int carsNumber, ILine line)
        {
            Id = id;
            CarsNumber = carsNumber;
            Line = line;

            Fields = new Dictionary<string, Func<object>>()
            {
                ["id"] = () => Id,
                ["carsNumber"] = () => CarsNumber
            };
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Tram #").Append(Id).Append(", cars: ").Append(CarsNumber).Append(", line: ").Append(Line?.NumberDec.ToString() ?? "none");
            return builder.ToString();
        }
    }
}