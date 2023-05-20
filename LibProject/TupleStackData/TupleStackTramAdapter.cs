﻿using System;
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
            set => _adaptee.TupleRepr = Tuple.Create(value, _adaptee.TupleRepr.Item2);
        }

        public override int CarsNumber
        {
            get => int.Parse(_adaptee["carsNumber"]);
            set => _adaptee["carsNumber"] = value.ToString();
        }

        public override Line Line
        {
            get
            {
                List<string> fromStack = _adaptee.TupleRepr.Item2.ToList();
                int i = fromStack.FindIndex(x => x.Equals("line"));

                return TupleStackRepresentation.Lines[int.Parse(fromStack[i + 2])];
            }
        }

        public TupleStackTramAdapter(TupleStackTram adaptee)
        {
            this._adaptee = adaptee;

            TupleStackRepresentation.Trams.Add(adaptee.TupleRepr.Item1, this);
            TupleStackRepresentation.Vehicles.Add(adaptee.TupleRepr.Item1, this);
        }
    }
}