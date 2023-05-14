using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BTM.TextData
{
    public sealed class TextStopAdapter : Stop
    {
        private static Regex _stop = new Regex(@"#(?<id>\d+)\((?:(?<lineid>\d+),?)+\)(?<name>.+)/(?<type>\w+)", RegexOptions.Compiled);

        private readonly TextStop _adaptee;

        public override int Id
        {
            get => int.Parse(_stop.Match(_adaptee.TextRepr).Groups["id"].Value);
            set => throw new NotImplementedException();
        }

        public override List<Line> Lines => _stop.Match(_adaptee.TextRepr).Groups["lineid"]
            .Captures.Select(id => TextRepresentation.Lines[int.Parse(id.Value)]).ToList();

        public override string Name
        {
            get => _stop.Match(_adaptee.TextRepr).Groups["name"].Value;
            set => throw new NotImplementedException();
        }

        public override string Type
        {
            get => _stop.Match(_adaptee.TextRepr).Groups["type"].Value;
            set => throw new NotImplementedException();
        }

        public TextStopAdapter(TextStop adaptee)
        {
            this._adaptee = adaptee;

            TextRepresentation.Stops.Add(Id, this);
        }
    }
}