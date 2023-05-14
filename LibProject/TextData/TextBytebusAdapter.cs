using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BTM.Refraction;

namespace BTM.TextData
{
    public sealed class TextBytebusAdapter : Bytebus
    {
        private static Regex _bytebus = new Regex(@"#(?<id>\d+)\^(?<engineclass>.+)\*(?:(?<lineid>\d+),?)+", RegexOptions.Compiled);

        private readonly TextBytebus _adaptee;

        public override int Id
        {
            get => int.Parse(_bytebus.Match(_adaptee.TextRepr).Groups["id"].Value);
            set => throw new NotImplementedException();
        }

        public override List<Line> Lines => _bytebus.Match(_adaptee.TextRepr).Groups["lineid"]
            .Captures.Select(id => TextRepresentation.Lines[int.Parse(id.Value)]).ToList();

        public override string EngineClass
        {
            get => _bytebus.Match(_adaptee.TextRepr).Groups["engineclass"].Value;
            set => throw new NotImplementedException();
        }

        public TextBytebusAdapter(TextBytebus adaptee)
        {
            this._adaptee = adaptee;

            TextRepresentation.Bytebuses.Add(Id, this);
            TextRepresentation.Vehicles.Add(Id, this);
        }
    }
}