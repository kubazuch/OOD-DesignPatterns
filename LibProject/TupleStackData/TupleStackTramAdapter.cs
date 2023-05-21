using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public class TupleStackTramAdapter : Tram
    {
        private readonly TupleStackTram _adaptee;

        public override int Id
        {
            get => _adaptee.TupleRepr.Item1;
            set
            {
                ChangeId(value);
                _adaptee.TupleRepr = Tuple.Create(value, _adaptee.TupleRepr.Item2);
            }
        }

        public override int CarsNumber
        {
            get => int.Parse(_adaptee["carsNumber"]);
            set => _adaptee["carsNumber"] = value.ToString();
        }

        public override Line? Line
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("line"));
                try
                {
                    return Vault.Lines[int.Parse(fromStack[i + 2])];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ArgumentException("Reference to nonexistent Line", ex);
                }
            }
            protected set => _adaptee["line"] = (value?.NumberDec ?? -1).ToString();
        }

        public TupleStackTramAdapter(TupleStackTram adaptee)
        {
            this._adaptee = adaptee;
        }

        public override void OnLineDeleted(Line line)
        {
        }
    }
}