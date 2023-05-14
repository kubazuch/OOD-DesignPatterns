using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public class TextDriverAdapter : Driver
    {
        private static Regex _driver = new Regex(@"(?<name>\w+) (?<surname>\w+)\((?<seniority>\d+)\)@(?:(?<vehicleid>\d+),?)+", RegexOptions.Compiled);

        private readonly TextDriver _adaptee;

        public override List<Vehicle> Vehicles => _driver.Match(_adaptee.TextRepr).Groups["vehicleid"]
            .Captures.Select(id => TextRepresentation.Vehicles[int.Parse(id.Value)]).ToList();
        
        public override string Name
        {
            get => _driver.Match(_adaptee.TextRepr).Groups["name"].Value;
            set => throw new NotImplementedException();
        }

        public override string Surname
        {
            get => _driver.Match(_adaptee.TextRepr).Groups["surname"].Value;
            set => throw new NotImplementedException();
        }

        public override int Seniority
        {
            get => int.Parse(_driver.Match(_adaptee.TextRepr).Groups["seniority"].Value);
            set => throw new NotImplementedException();
        }

        public TextDriverAdapter(TextDriver adaptee)
        {
            this._adaptee = adaptee;

            TextRepresentation.Drivers.Add(TextRepresentation.Drivers.Count, this);
        }
    }
}
