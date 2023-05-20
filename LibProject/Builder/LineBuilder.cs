using System.Collections.Generic;
using System.Linq;
using BTM.Refraction;

namespace BTM.Builder
{
    public class LineBuilder : AbstractBuilder
    {
        public static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<string?>("numberHex"), new Field<int?>("numberDec"), new Field<string?>("commonName")
        }.ToDictionary(f => f.Name, f => f);

        internal string? NumberHex;
        internal int? NumberDec;
        internal string? CommonName;

        public LineBuilder(bool init = true) : base("line", AvailableFields)
        {
            AssignSettersAndGetters();

            if(!init) return;

            NumberHex = "0";
            NumberDec = 0;
            CommonName = "Bus line";
        }

        public sealed override void AssignSettersAndGetters()
        {
            var numberHex = (Field<string?>)Fields["numberHex"];
            numberHex.SetSetter(value => NumberHex = value);
            numberHex.SetGetter(() => NumberHex);

            var numberDec = (Field<int?>)Fields["numberDec"];
            numberDec.SetSetter(value => NumberDec = value);
            numberDec.SetGetter(() => NumberDec);

            var commonName = (Field<string?>)Fields["commonName"];
            commonName.SetSetter(value => CommonName = value);
            commonName.SetGetter(() => CommonName);
        }

        public LineBuilder SetNumberHex(string numberHex)
        {
            this.NumberHex = numberHex;
            return this;
        }

        public LineBuilder SetNumberDec(int numberDec)
        {
            this.NumberDec = numberDec;
            return this;
        }

        public LineBuilder SetCommonName(string commonName)
        {
            this.CommonName = commonName;
            return this;
        }

        public override Entity Build(AbstractFactory abstractFactory) => abstractFactory.CreateLine(this);
    }
}
