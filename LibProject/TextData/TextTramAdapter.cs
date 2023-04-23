﻿using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public class TextTramAdapter : TextVehicleAdapter, ITram
    {
        private static Regex TRAM = new Regex(@"#(?<id>\d+)\((?<carsNumber>.+)\)(?<lineid>\d+)", RegexOptions.Compiled);
        
        private TextTram adaptee;

        public override int Id => int.Parse(TRAM.Match(adaptee.TextRepr).Groups["id"].Value);
        public int CarsNumber => int.Parse(TRAM.Match(adaptee.TextRepr).Groups["carsNumber"].Value);
        public ILine Line => TextRepresentation.LINES[int.Parse(TRAM.Match(adaptee.TextRepr).Groups["lineid"].Value)];

        public TextTramAdapter(TextTram adaptee)
        {
            this.adaptee = adaptee;

            TextRepresentation.TRAMS.Add(Id, this);
            TextRepresentation.VEHICLES.Add(Id, this);
        }

        public override object GetValueByName(string name)
        {
            switch (name)
            {
                case "id":
                    return Id;
                case "carsNumber":
                    return CarsNumber;
                default:
                    throw new ArgumentException($"Unknown field: {name}");
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Tram #").Append(Id).Append(", cars: ").Append(CarsNumber).Append(", line: ").Append(Line.NumberDec);
            return builder.ToString();
        }
    }
}