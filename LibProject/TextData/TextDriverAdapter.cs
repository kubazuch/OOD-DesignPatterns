using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public class TextDriverAdapter : IDriver
    {
        private static Regex DRIVER = new Regex(@"(?<name>\w+) (?<surname>\w+)\((?<seniority>\d+)\)@(?:(?<vehicleid>\d+),?)+", RegexOptions.Compiled);

        private TextDriver adaptee;

        public List<IVehicle> Vehicles => DRIVER.Match(adaptee.TextRepr).Groups["vehicleid"]
            .Captures.Select(id => TextRepresentation.VEHICLES[int.Parse(id.Value)]).ToList();
        public string Name => DRIVER.Match(adaptee.TextRepr).Groups["name"].Value;
        public string Surname => DRIVER.Match(adaptee.TextRepr).Groups["surname"].Value;
        public int Seniority => int.Parse(DRIVER.Match(adaptee.TextRepr).Groups["seniority"].Value);

        public TextDriverAdapter(TextDriver adaptee)
        {
            this.adaptee = adaptee;

            Fields = new Dictionary<string, Func<object>>
            {
                ["name"] = () => Name,
                ["surname"] = () => Surname,
                ["seniority"] = () => Seniority
            };

            TextRepresentation.DRIVERS.Add(TextRepresentation.DRIVERS.Count, this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name).Append(' ').Append(Surname).Append(", ").Append(Seniority).AppendLine(" years of seniority");
            builder.Append("\tVehicles: [").AppendJoin(", ", Vehicles.Select(x => x.Id)).Append(']');
            return builder.ToString();
        }

        public Dictionary<string, Func<object>> Fields { get; }
    }
}
