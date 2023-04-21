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

            HashMap<int> map = new (10)
            {
                13,
                4,
                6,
                7,
                1
            };

            Console.WriteLine("All elements from beginning:");
            var forwardIter = map.GetForwardIterator();
            while (forwardIter.MoveNext())
                Console.WriteLine(forwardIter.Current);
            Console.WriteLine("All elements from end:");
            var reverseIter = map.GetReverseIterator();
            while (reverseIter.MoveNext())
                Console.WriteLine(reverseIter.Current);

            Console.WriteLine("Last even element in map (should be: 6):");
            Console.WriteLine(Algorithms.Find(map.GetReverseIterator(), x => x % 2 == 0));

            // Try changing the value
            Algorithms.ForEach(map.GetForwardIterator(), x => x = x + 1);

            Console.WriteLine("All elements in map from beginning:");
            Algorithms.ForEach(map.GetForwardIterator(), Console.WriteLine);

            Console.WriteLine("Count of odd numbers:");
            Console.WriteLine(Algorithms.CountIf(map.GetForwardIterator(), x => x % 2 != 0));
        }
    }
}
