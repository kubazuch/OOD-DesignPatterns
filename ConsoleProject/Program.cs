using System;
using System.Collections.Generic;
using System.Linq;
using BTM;
using BTM.Collections;

namespace ConsoleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataManager manager = new DataManager(DataRepresentation.TUPLE_STACK);

            Console.WriteLine("Zadnie 2: Wypisać linie (numer i nazwa) na których jeżdżą pojazdy kierowane przez kierowców mających przynajmniej 10 lat stażu");
            foreach (var line in manager.Lines.Where(line => manager.Drivers.Any(d => d.Seniority >= 10 && d.Vehicles.Intersect(line.Vehicles).Any())))
                Console.WriteLine($"    Line #{line.NumberDec}: {line.CommonName}");
        }
    }
}
