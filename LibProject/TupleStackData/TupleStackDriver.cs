using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.TupleStackData
{
    public class TupleStackDriver : TupleStackRepresentation
    {
        private static int _id = 0;

        public override Tuple<int, Stack<string>> TupleRepr { get; set; }

        public TupleStackDriver(string name, string surname, int seniority, params int[] vehicles)
        {
            List<string> toStack = new List<string>();

            toStack.Add("name");
            toStack.Add("1");
            toStack.Add(name);

            toStack.Add("surname");
            toStack.Add("1");
            toStack.Add(surname);

            toStack.Add("seniority");
            toStack.Add("1");
            toStack.Add(seniority.ToString());

            toStack.Add("vehicles");
            toStack.Add(vehicles.Length.ToString());
            toStack.AddRange(vehicles.Select(x => x.ToString()));

            toStack.Reverse();
            TupleRepr = Tuple.Create(_id++, new Stack<string>(toStack));
        }
    }
}