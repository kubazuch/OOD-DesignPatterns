using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public class TextTramAdapter : TextVehicleAdapter, ITram
    {
        private static Regex TRAM = new Regex(@"#(?<id>\d+)\((?<carsNumber>.+)\)(?<lineid>\d+)", RegexOptions.Compiled);
        
        private TextTram adaptee;

        public override int Id => int.Parse(TRAM.Match(adaptee.TextRepr).Groups["id"].Value);
        public override Dictionary<string, Func<object>> Fields { get; }
        public int CarsNumber => int.Parse(TRAM.Match(adaptee.TextRepr).Groups["carsNumber"].Value);
        public ILine Line => TextRepresentation.LINES[int.Parse(TRAM.Match(adaptee.TextRepr).Groups["lineid"].Value)];

        public TextTramAdapter(TextTram adaptee)
        {
            this.adaptee = adaptee;

            Fields = new Dictionary<string, Func<object>>()
            {
                ["id"] = () => Id,
                ["carsNumber"] = () => CarsNumber
            };

            TextRepresentation.TRAMS.Add(Id, this);
            TextRepresentation.VEHICLES.Add(Id, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Tram #").Append(Id).Append(", cars: ").Append(CarsNumber).Append(", line: ").Append(Line.NumberDec);
            return builder.ToString();
        }
    }
}