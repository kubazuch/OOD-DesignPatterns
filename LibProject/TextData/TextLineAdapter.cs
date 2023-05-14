using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public sealed class TextLineAdapter : Line
    {
        private static Regex _line = new Regex(@"(?<numerHex>[0-9A-Z]+)\((?<numerDec>[0-9]+)\)`(?<commonName>.+)`@(?:(?<stopid>\d+),?)+!(?:(?<vehicleid>\d+),?)+", RegexOptions.Compiled);

        private readonly TextLine _adaptee;

        public override string NumberHex
        {
            get => _line.Match(_adaptee.TextRepr).Groups["numerHex"].Value;
            set => throw new NotImplementedException();
        }

        public override int NumberDec
        {
            get => int.Parse(_line.Match(_adaptee.TextRepr).Groups["numerDec"].Value);
            set => throw new NotImplementedException();
        }

        public override string CommonName
        {
            get => _line.Match(_adaptee.TextRepr).Groups["commonName"].Value;
            set => throw new NotImplementedException();
        }

        public override List<Stop> Stops => _line.Match(_adaptee.TextRepr).Groups["stopid"]
            .Captures.Select(id => TextRepresentation.Stops[int.Parse(id.Value)]).ToList();

        public override List<Vehicle> Vehicles => _line.Match(_adaptee.TextRepr).Groups["vehicleid"]
            .Captures.Select(id => TextRepresentation.Vehicles[int.Parse(id.Value)]).ToList();

        public TextLineAdapter(TextLine adaptee)
        {
            this._adaptee = adaptee;

            TextRepresentation.Lines.Add(NumberDec, this);
        }
    }
}