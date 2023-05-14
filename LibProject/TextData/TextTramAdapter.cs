using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public sealed class TextTramAdapter : Tram
    {
        private static Regex _tram = new Regex(@"#(?<id>\d+)\((?<carsNumber>.+)\)(?<lineid>\d+)", RegexOptions.Compiled);

        private readonly TextTram _adaptee;

        public override int Id
        {
            get => int.Parse(_tram.Match(_adaptee.TextRepr).Groups["id"].Value);
            set => throw new NotImplementedException();
        }
        
        public override int CarsNumber
        {
            get => int.Parse(_tram.Match(_adaptee.TextRepr).Groups["carsNumber"].Value);
            set => throw new NotImplementedException();
        }

        public override Line Line => TextRepresentation.Lines[int.Parse(_tram.Match(_adaptee.TextRepr).Groups["lineid"].Value)];

        public TextTramAdapter(TextTram adaptee)
        {
            this._adaptee = adaptee;

            TextRepresentation.Trams.Add(Id, this);
            TextRepresentation.Vehicles.Add(Id, this);
        }
    }
}